using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Interfaces.Data;
using Forum.Models;
using Forum.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ForumRole.Admin)]
    public class CategoryController : Controller
    {
        private IWebHostEnvironment _env;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IWebHostEnvironment env, IUnitOfWork unitOfWork)
        {
            _env = env;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();

            return View(categories);
        }

        //GET - Create
        public IActionResult Create()
        {
            return View();
        }


        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }


            _unitOfWork.Category.Insert(category);
            await _unitOfWork.SaveAsync();

            string webRootPath = _env.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var categoryFromDB = await _unitOfWork.Category.GetByIdAsync(category.Id);

            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, @"img\category_img");
                var extension = Path.GetExtension(files[0].FileName);

                using (var flieStream = new FileStream(Path.Combine(uploads, category.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(flieStream);
                }

                categoryFromDB.Image = @"\img\category_img\" + category.Id + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPath, @"img\category_img\" + "default_img_category");
                System.IO.File.Copy(uploads, webRootPath + @"\img\category_img\" + category.Id + ".png");
                categoryFromDB.Image = @"\img\category_img\" + category.Id + ".png";
            }

            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var categoryFromDB = await _unitOfWork.Category.GetByIdAsync(id.Value);

            if (categoryFromDB == null)
                return NotFound();

            return View(categoryFromDB);
        }


        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (category == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            string webRootPath = _env.WebRootPath;
            var files = HttpContext.Request.Form.Files;


            var categoryFromDB = await _unitOfWork.Category.GetByIdAsync(category.Id);

            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, @"img\category_img");
                var extension_new = Path.GetExtension(files[0].FileName);

                var imgToDel = Path.Combine(webRootPath, categoryFromDB.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imgToDel))
                {
                    System.IO.File.Delete(imgToDel);
                }

                using (var flieStream = new FileStream(Path.Combine(uploads, category.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(flieStream);
                }

                categoryFromDB.Image = @"\img\category_img\" + category.Id + extension_new;
            }

            categoryFromDB.Name = category.Name;
            categoryFromDB.Description = category.Description;

            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));

        }

        //GET - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var categoryFromDB = await _unitOfWork.Category.GetByIdAsync(id.Value);

            if (categoryFromDB == null)
                return NotFound();

            return View(categoryFromDB);
        }


        //POST - Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
                return NotFound();

            var categoryFromDB = await _unitOfWork.Category.GetByIdAsync(id.Value);

            if (categoryFromDB == null)
                return NotFound();

            string webRootPath = _env.WebRootPath;

            var imgToDel = Path.Combine(webRootPath, categoryFromDB.Image.TrimStart('\\'));

            if (System.IO.File.Exists(imgToDel))
            {
                System.IO.File.Delete(imgToDel);
            }

            var topicsFromCategory = await _unitOfWork.Topic.GetAllAsync(m => m.CategoryId == categoryFromDB.Id);

            if (topicsFromCategory != null)
                _unitOfWork.Topic.RemoveRange(topicsFromCategory);

            _unitOfWork.Category.Remove(categoryFromDB);

            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
