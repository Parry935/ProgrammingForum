using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Forum.Models;
using Forum.Data;
using Microsoft.EntityFrameworkCore;
using Forum.Interfaces.Data;
using Forum.ViewModel;

namespace Forum.Controllers
{

    [Area("CustomUser")]
    public class HomeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();

            var homeVM = new List<HomeVM>();

            foreach (var category in categories)
            {
                homeVM.Add(new HomeVM(){
                    Category = category,
                    LastPost = await _unitOfWork.Post.GetLastPostForCategory(category.Id)}
                );
            }
            

            return View(homeVM);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
