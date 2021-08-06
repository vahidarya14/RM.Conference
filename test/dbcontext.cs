using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;

namespace test
{
    public class AppDbcontext: IdentityDbContext<User, IdentityRole<long>, long>
    {
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Sponser> Sponsers { get; set; }
        public DbSet<SlideShow> SlideShows { get; set; }
        public DbSet<News> Newses { get; set; }

        public AppDbcontext(DbContextOptions<AppDbcontext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sponser>(x=> {
                x.Property(y => y.Email).HasMaxLength(60);
                x.Property(y => y.Link).HasMaxLength(50);
                x.Property(y => y.Phone).HasMaxLength(50);
                x.Property(y => y.Title).HasMaxLength(50);
                x.Property(y => y.Logo).HasMaxLength(50);
            });

            modelBuilder.Entity<Speaker>(x => {
                x.Property(y => y.FullName).HasMaxLength(60);
                x.Property(y => y.Facebook).HasMaxLength(50);
                x.Property(y => y.Phone).HasMaxLength(50);
                x.Property(y => y.Instagram).HasMaxLength(50);
                x.Property(y => y.Degree).HasMaxLength(50);
                x.Property(y => y.Linkedin).HasMaxLength(50);
                x.Property(y => y.Avatar).HasMaxLength(50);
                x.Property(y => y.Twitter).HasMaxLength(50);
                x.Property(y => y.Email).HasMaxLength(50);
            });

            modelBuilder.Entity<Conference>(x => {
                x.Property(y => y.Title).HasMaxLength(60);
                x.Property(y => y.CityName).HasMaxLength(50);
                x.Property(y => y.PlaceName).HasMaxLength(150);
                x.Property(y => y.Bg).HasMaxLength(50);
                x.Property(y => y.AddsVideo).HasMaxLength(50);
            });

            modelBuilder.Entity<ConferenceSponserRequest>(x => {
                x.Property(y => y.Email).HasMaxLength(60);
                x.Property(y => y.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<Setting>(x => {
                //x.HasNoKey();
                x.Property(y => y.Theme).HasMaxLength(60);
                x.Property(y => y.SiteLogo).HasMaxLength(60);
                x.Property(y => y.SiteName).HasMaxLength(60);
            });

            modelBuilder.Entity<SlideShow>(x => {
                x.Property(y => y.Url).HasMaxLength(60);
                x.Property(y => y.Description).HasMaxLength(300);
                x.Property(y => y.Title).HasMaxLength(60);
                x.Property(y => y.Img).HasMaxLength(60);
            });

            modelBuilder.Entity<News>(x => {
                x.Property(y => y.Title).HasMaxLength(60);
                x.Property(y => y.Avatar).HasMaxLength(60);
            });

            base.OnModelCreating(modelBuilder);

            Seed(modelBuilder);
        }


        void Seed( ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasData(
               new Setting { Id = 1, Theme = "" }
           );

            modelBuilder.Entity<Speaker>().HasData(
                new Speaker
                {
                    Id = 1,
                    FullName = "William Shakespeare",
                    Degree = "bachelor"
                }
            );
            modelBuilder.Entity<Sponser>().HasData(
                new Sponser { Id = 1, Title = "Hamlet" },
                new Sponser { Id = 2, Title = "King Lear" },
                new Sponser { Id = 3, Title = "Othello" }
            );
        }
    }

    public class Conference
    {
        public long Id { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public string Title { get; set; }
        public string CityName { get; set; }
        public string PlaceName { get; set; }
        public string Slug =>string.IsNullOrWhiteSpace(Title)?"": Title.Replace(" ", "-");
        public string Bg { get; set; }
        public string AddsVideo { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public List<ConferenceSponser> Sponsers { get; set; } = new List<ConferenceSponser>();
        public List<ConferenceSpeaker> Speakers { get; set; } = new List<ConferenceSpeaker>();
        public List<User> RegisterdParticipant { get; set; } = new List<User>();
        public List<ConferenceSponserRequest> SponserRequests { get; set; } = new List<ConferenceSponserRequest>();

        
    }

    public class ConferenceSponser
    {
        public long Id { get; set; }
        public long ConferenceId { get; set; }
        public Conference Conference { get; set; }
        public Sponser Sponser { get; set; }
        public long SponserId { get; set; }
    }

    public class ConferenceSpeaker
    {
        public long Id { get; set; }
        public long ConferenceId { get; set; }
        public Conference Conference { get; set; }
        public Speaker Speaker { get; set; }
        public long SpeakerId { get; set; }
    }

    public class Sponser
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Logo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public List<ConferenceSponser> Coferences { get; set; } = new List<ConferenceSponser>();
    }

    public class Speaker
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Degree { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Instagram { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        
        public List<ConferenceSpeaker> Coferences { get; set; } = new List<ConferenceSpeaker>();
    }
 
    public class User: IdentityUser<long>
    {
        public string  Name { get; set; }
        public string Family { get; set; }
    }

    public class ConferenceSponserRequest
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public long ConferenceId { get; set; }
        public DateTime RequestDateTime { get; set; }

        public Conference Conference { get; set; }
    }


    public class Setting
    {
        public int Id { get; set; }
        public string Theme { get; set; }
        public string SiteName { get; set; }
        public string SiteLogo { get; set; }
    }

    public class SlideShow
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class News
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long WriterId { get; set; }

        public User Writer { get; set; }
    }


}
