using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace test
{
    [Authorize]
    public class BaseController: Controller
    {
       protected AppDbcontext Db;
        public BaseController(AppDbcontext db)
        {
            Db = db;
        }

        protected async Task<List<(bool IsSuccess, string fileName, string msg)>> UpLoadFiles(string folder)
        {
            var lst = new List<(bool IsSuccess, string fileName, string msg)>();
            if (Request.Form.Files.Count > 0)
            {
                for (int i = 0; i < Request.Form.Files.Count; i++)
                {
                    var file = Request.Form.Files[i];
                    if (file == null || file.Length <= 0)
                    {
                        lst.Add((false, "", "فایل نامعتبر است"));
                        return lst;
                    }


                    var fileName = Path.GetFileName(file.FileName);
                    fileName = Guid.NewGuid() + fileName;

                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    var path = Path.Combine(folder, fileName);
                    await using (var stream = new FileStream(Path.Combine(folder, path), FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    lst.Add((true, fileName, ""));
                }

            }
            else
            {
                lst.Add((false, "", "فایلی ارسال نشده است"));
            }
            return lst;
        }

        //IWebHostEnvironment host
        protected async Task<(bool IsSuccess, string fileName, string msg)> UpLoadFile(string folder)
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                if (file == null || file.Length <= 0)
                {
                    return (false, "", "فایل نامعتبر است");
                }


                var fileName = Path.GetFileName(file.FileName);
                fileName = Guid.NewGuid() + fileName;

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var path = Path.Combine(folder, fileName);
                await using (var stream = new FileStream(Path.Combine(folder, path), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return (true, fileName, "");


            }

            return (false, "", "فایلی ارسال نشده است");
        }



    }
}
