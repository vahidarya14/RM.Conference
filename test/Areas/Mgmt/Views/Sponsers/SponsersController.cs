using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Areas.Mgmt.Controllers
{
    public class SponsersController : BaseController
    {
        public SponsersController(AppDbcontext db) : base(db)
        {
        }


        public IActionResult Index()
        {
            return View(Db.Sponsers);
        }



        public async Task<IActionResult> CreateEdit(long? id)
        {
            Sponser item = id.HasValue ? await Db.Sponsers.Include(x => x.Coferences).ThenInclude(x => x.Conference).FirstOrDefaultAsync(x => x.Id == id.Value) : new();
            if (item == null) item = new();

            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEdit(Sponser item2, [FromServices] IWebHostEnvironment host)
        {
            var isEdit = item2.Id > 0;
            Sponser item = item2.Id > 0 ? await Db.Sponsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == item2.Id) : null;

            var aa = await UpLoadFile($"{host.WebRootPath}/images/sponsors");
            if (aa.IsSuccess)
                item2.Logo = aa.fileName;

            if (item == null)
                await Db.Sponsers.AddAsync(item2);
            else
                Db.Sponsers.Update(item2);

            var res = await Db.SaveChangesAsync();

            if ((isEdit && res >= 0) || res >= 1)
                return RedirectToAction(nameof(Index));

            return View(item2);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
