using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using test.Models;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        AppDbcontext Db;
        public HomeController(AppDbcontext db, ILogger<HomeController> logger)
        {
            Db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["confs"] = await Db.Conferences.Where(x=>x.FromDateTime>DateTime.Today).OrderBy(x=>x.FromDateTime).Take(5).ToListAsync();
            return View();
        }

        [HttpGet("Conference/{year:int}/{month:int}/{day:int}/{slug}")]
        public async Task<IActionResult> Conference(int year, int month, int day, string slug)
        {
            var conf = await Db.Conferences
                 .Include(x => x.SponserRequests)
                 .Include(x => x.Sponsers).ThenInclude(x => x.Sponser)
                 .Include(x => x.Speakers).ThenInclude(x => x.Speaker)
                 .FirstOrDefaultAsync(x => x.Title.Replace(" ", "-") == slug && x.FromDateTime.Year == year && x.FromDateTime.Month == month && x.FromDateTime.Day == day);
                 //.FirstOrDefaultAsync(x => x.Id == id);
            return View(conf);
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
