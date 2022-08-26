using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using test;

namespace Conference.Areas.Mgmt.Views
{
    public class SettingsController : BaseController
    {
        IMemoryCache _cache;

        public SettingsController(AppDbcontext db, IMemoryCache cache) : base(db)
        {
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            
            return View((await Db.Setting.FirstOrDefaultAsync()??new Setting()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAsync(Setting setting)
        {
            var oldSetting = (await Db.Setting.FirstOrDefaultAsync()??new Setting());
            oldSetting.Theme = setting.Theme;
            Db.Setting.Update(oldSetting);
            var res =await Db.SaveChangesAsync();

            _cache.Set("setting", setting);
            return RedirectToAction(nameof(Index));
        }


    }
}
