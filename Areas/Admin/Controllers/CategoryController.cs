using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment _env;

        public CategoryController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _db.Category.ToArrayAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }


            _db.Category.Add(category);
            await _db.SaveChangesAsync();

            string webRootPath = _env.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var categoryFromDB = await _db.Category.FindAsync(category.Id);

            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, "img");
                var extension = Path.GetExtension(files[0].FileName);

                using (var flieStream = new FileStream(Path.Combine(uploads, category.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(flieStream);
                }

                categoryFromDB.Image = @"\img\" + category.Id + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPath, @"img\" + "default_img_category");
                System.IO.File.Copy(uploads, webRootPath + @"\img\" + category.Id + ".png");
                categoryFromDB.Image = @"\img\" + category.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
