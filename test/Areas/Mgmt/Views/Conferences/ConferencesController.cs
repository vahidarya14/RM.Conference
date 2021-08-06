using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Areas.Mgmt.Controllers
{
    public class ConferencesController : BaseController
    {
        public ConferencesController(AppDbcontext db) : base(db)
        {
        }

        public IActionResult Index()
        {
            return View(
                Db.Conferences
                .Include(x => x.SponserRequests)
                .Include(x => x.Sponsers).ThenInclude(x => x.Sponser)
                .Include(x => x.Speakers).ThenInclude(x => x.Speaker)
                );
        }

        public async Task<IActionResult> CreateEdit(long? id)
        {
            ViewData["Speakers"] = await Db.Speakers.Select(x => new Speaker { FullName = x.FullName, Id = x.Id }).ToListAsync();
            ViewData["Sponsers"] = await Db.Sponsers.Select(x => new Sponser { Title = x.Title, Id = x.Id }).ToListAsync();

            Conference item = id.HasValue ? await Db.Conferences
                .Include(x => x.SponserRequests)
                .Include(x => x.Sponsers).ThenInclude(x => x.Sponser)
                .Include(x => x.Speakers).ThenInclude(x => x.Speaker)
                .FirstOrDefaultAsync(x => x.Id == id.Value) : new Conference
                {
                    FromDateTime = DateTime.Now.AddDays(31),
                    ToDateTime = DateTime.Now.AddDays(32),
                    Lat = 35.6892,
                    Lng = 51.3890
                };
            if (item == null) item = new Conference
            {
                FromDateTime = DateTime.Now.AddDays(31),
                ToDateTime = DateTime.Now.AddDays(32),
                Lat = 35.6892,
                Lng = 51.3890
            };

            return View(item);
        }


        [HttpPost]
        public async Task<IActionResult> CreateEdit(Conference item, List<long> speakers, List<long> sponsers)
        {
            item.FromDateTime = item.FromDateTime.ToGeogrian();
            item.ToDateTime = item.ToDateTime.ToGeogrian();

            Conference oldItem = item.Id >= 0 ? await Db.Conferences
               .Include(x => x.SponserRequests)
               .Include(x => x.Sponsers).ThenInclude(x => x.Sponser)
               .Include(x => x.Speakers).ThenInclude(x => x.Speaker)
               .FirstOrDefaultAsync(x => x.Id == item.Id) : new Conference { };
            _ = $"{123.4567:0.00}";
            if (item.Id == 0)
            {
                foreach (var s in speakers)
                {
                    item.Speakers.Add(new ConferenceSpeaker { Conference = item, SpeakerId = s });
                }
                foreach (var s in sponsers)
                {
                    item.Sponsers.Add(new ConferenceSponser { Conference = item, SponserId = s });
                }

                await Db.Conferences.AddAsync(item);
            }
            else
            {
                oldItem.FromDateTime = item.FromDateTime;
                oldItem.ToDateTime = item.ToDateTime;
                oldItem.Title = item.Title;
                oldItem.Lat = item.Lat;
                oldItem.Lng = item.Lng;
                oldItem.CityName = item.CityName;
            }

            try
            {
                var res = await Db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
            }


            return View(item);
        }

    }
}
