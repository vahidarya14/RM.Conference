﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Areas.Mgmt.Controllers
{
    public class SpeakersController : BaseController
    {
        public SpeakersController(AppDbcontext db) : base(db)
        {
        }

        public IActionResult Index()
        {
            return View(Db.Speakers);
        }

        public async Task<IActionResult> CreateEdit(long? id)
        {
            Speaker item = id.HasValue ? await Db.Speakers
                .Include(x => x.Coferences).ThenInclude(x => x.Conference)
                .FirstOrDefaultAsync(x => x.Id == id.Value) : new();
            if (item == null) item = new();

            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEdit(Speaker item2,[FromServices] IWebHostEnvironment host)
        {
            var isEdit = item2.Id > 0;
            Speaker item = item2.Id > 0 ? await Db.Speakers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == item2.Id) : null;

            var aa = await UpLoadFile($"{host.WebRootPath}/images/speakers");
            if (aa.IsSuccess)
                item2.Avatar = aa.fileName;

            if (item == null)
                await Db.Speakers.AddAsync(item2);
            else
                Db.Speakers.Update(item2);
                         

            var res = await Db.SaveChangesAsync();

            if ((isEdit && res >= 0) || res >= 1)
                return RedirectToAction(nameof(Index));

            return View(item2);
        }


    }
}
