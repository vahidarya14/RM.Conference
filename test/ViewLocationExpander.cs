using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using test;

namespace Conference
{
    public static class HttpContextExt
    {
        public static string GetSetting(this HttpContext httpContext)
        {
            var _cache = (IMemoryCache)httpContext.RequestServices.GetService(typeof(IMemoryCache));
            var setting = _cache.Get<Setting>("setting");
            if (setting != null)
                return setting.Theme;

            var context = (AppDbcontext)httpContext.RequestServices.GetService(typeof(AppDbcontext));
            setting = context.Setting.FirstOrDefault();

            if (setting != null)
                _cache.Set("setting", setting);
            else
                _cache.Set("setting", new Setting { Theme = "" });


            return setting?.Theme ?? "";
        }
    }
    public class ViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "theme";

        public void PopulateValues(ViewLocationExpanderContext context)
        {

 context.Values[THEME_KEY] = context.ActionContext.HttpContext.GetSetting();


        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string theme = null;
            if (context.Values.TryGetValue(THEME_KEY, out theme))
            {
                if (!string.IsNullOrWhiteSpace(theme))
                {
                    IEnumerable<string> themeLocations = new[]
                    {
                        $"/Themes/{theme}/{{1}}/{{0}}.cshtml",
                        $"/Themes/{theme}/Shared/{{0}}.cshtml"
                    };

                    var areas = viewLocations.Where(x => x.StartsWith("/Areas/")).ToList();
                    var others = viewLocations.Where(x =>! x.StartsWith("/Areas/")).ToList();
                    viewLocations = areas.Concat( themeLocations.Concat(others)).ToList();
                }

                //string tenant;
                //if (context.Values.TryGetValue(TENANT_KEY, out tenant))
                //{
                //    themeLocations = ExpandTenantLocations(tenant, themeLocations);
                //}

            }


            return viewLocations;
        }




    }
}
