using Conference;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace test
{
    public class Startup
    {
        IConfiguration _configuration;
        IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mvc=services.AddControllersWithViews();
            if (_environment.IsDevelopment())
            {
                mvc.AddRazorRuntimeCompilation();
            }


            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            services.AddIdentity<User, IdentityRole<long>>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDbcontext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/account/login";
                options.LogoutPath = $"/account/logout";
                options.AccessDeniedPath = $"/account/accessDenied";
                options.Cookie.Name = "RM.Conference._ah";
                options.Cookie.HttpOnly = false;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(300);
                //// ReturnUrlParameter requires 
                ////using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });


            services.AddDbContext<AppDbcontext>(builder =>
            {
                builder.UseSqlServer(_configuration.GetConnectionString("Conn"));
            });

            services.AddRedMaskAuthorization(opt =>
            {
                opt.ClientId = "rm_conference";
                opt.ClientSecret = "secret";
                opt.CookieName = "RM.conference._ah";
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //var brokerOnMsgReciedHandler = app.ApplicationServices.GetService<IRabbitMqrOnMsgReceivedHandler>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapAreaControllerRoute(
                    "mgmt",
                    "mgmt",
                    "mgmt/{controller=Speakers}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("mgmt", "{area:mgmt}/{controller=Speakers}/{action=Index}/{id?}");

            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");


            //});
        }
    }

    //-----------------------------------------------
    public static class IServiceCollectionExt
    {
        public static string Cookies2 => "Cookies";
        public static string Oidc2 => "oidc";

        public static AuthenticationBuilder AddRedMaskAuthorization(this IServiceCollection Services, RedMaskOAuth2AuthenticationOptions opt)
        {


            return Services
                    .AddAuthentication(options => {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = Oidc2;
                        //این خیلی مهمه //برای اینکه AddOpenIdConnect و AddIdentity همزمان بشه استفاده بشه
                        options.DefaultAuthenticateScheme = Cookies2;
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                        options.Cookie.Name = opt.CookieName ?? "mvccode";
                    })
                    .AddOpenIdConnect(Oidc2, options => {

                        //این خیلی مهمه  //برای اینکه AddOpenIdConnect و AddIdentity همزمان بشه استفاده بشه
                        options.SignInScheme = Cookies2;

                        options.Authority = "https://ids.redmask.ir";
                        options.RequireHttpsMetadata = false;
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.ClientId = opt.ClientId;
                        options.ClientSecret = opt.ClientSecret;

                        // code flow + PKCE (PKCE is turned on by default)
                        options.ResponseType = "code";
                        //options.UsePkce = true;

                        options.Scope.Clear();
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.Scope.Add("myIdentity");
                        options.Scope.Add("roles");

                        // not mapped by default
                        options.ClaimActions.DeleteClaims("sub", "amr", "sid", "idp", "s_hash", "auth_time");
                        options.ClaimActions.MapUniqueJsonKey("phoneNumber", "phoneNumber");
                        options.ClaimActions.Add(new RoleClaimAction()); // <-- add this
                                                                         //options.ClaimActions.MapUniqueJsonKey("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Array");
                        options.ClaimActions.MapUniqueJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
                        options.ClaimActions.MapUniqueJsonKey("name", "name");
                        options.ClaimActions.MapUniqueJsonKey("family", "family");
                        options.ClaimActions.MapUniqueJsonKey("id", "id");
                        options.ClaimActions.MapUniqueJsonKey("email", "email");
                        options.ClaimActions.MapUniqueJsonKey("Avatar", "Avatar");
                        options.ClaimActions.MapUniqueJsonKey("FullName", "FullName");


                        // keeps id_token smaller
                        options.GetClaimsFromUserInfoEndpoint = true;
                        options.SaveTokens = true;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            //NameClaimType = "name",
                            //RoleClaimType = "role",
                        };

                        options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
                        {
                            OnTicketReceived = opt.OnTicketReceived,
                            //OnUserInformationReceived =opt.OnUserInformationReceived
                            OnRemoteFailure = opt.OnRemoteFailure
                        };
                    });
        }
        //public static AuthenticationBuilder AddRedMaskAuthorization(this IServiceCollection Services, string clientId, string clientSecret)
        //    => Services.AddRedMaskAuthorization(new RedMaskOAuth2AuthenticationOptions(clientId, clientSecret));

        public static AuthenticationBuilder AddRedMaskAuthorization(this IServiceCollection Services, Action<RedMaskOAuth2AuthenticationOptions> opt)
        {
            var option = new RedMaskOAuth2AuthenticationOptions();
            opt.Invoke(option);
            return Services.AddRedMaskAuthorization(option);
        }
    }

    public class RedMaskOAuth2AuthenticationOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CookieName { get; set; }
        public Func<TicketReceivedContext, Task> OnTicketReceived { get; set; } = (ctx) => Task.CompletedTask;
        public Func<RemoteFailureContext, Task> OnRemoteFailure { get; set; } = (ctx) => Task.CompletedTask;

        public RedMaskOAuth2AuthenticationOptions() { }
        public RedMaskOAuth2AuthenticationOptions(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }


    internal class ArrayClaimAction : ClaimAction
    {
        public ArrayClaimAction(string claimType, string valueType) : base(claimType, valueType)
        {
        }

        public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
        {
            List<string> roles = new();
            if (userData is JsonElement elt && elt.TryGetProperty(ClaimType, out elt))
            {
                roles = elt.ValueKind == JsonValueKind.String ? new List<string> { elt.ToString() } :
                        elt.EnumerateArray().Select(x => x.ToString()).ToList();
                foreach (var role in roles)
                {
                    Claim claim = new(ClaimType, role, ValueType, issuer);
                    if (!identity.HasClaim(c => c.Subject == claim.Subject && c.Value == claim.Value))
                    {
                        identity.AddClaim(claim);
                    }
                }
            }
        }
    }

    ////https://github.com/IdentityServer/IdentityServer4/issues/1786
    internal class RoleClaimAction : ArrayClaimAction
    {
        public RoleClaimAction() : base("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", ClaimValueTypes.String)
        {
        }


    }
}
