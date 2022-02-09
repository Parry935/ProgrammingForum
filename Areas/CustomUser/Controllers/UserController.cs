using Forum.Interfaces.Data;
using Forum.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Areas.CustomUser.Controllers
{

    [Area("CustomUser")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string name)
        {
            var user = await _unitOfWork.User
                .GetFirstOrDefaultAsync(m=>m.UserName == name);

            if(user == null)
                return NotFound();

            var userPosts = await _unitOfWork.Post
                .GetAllAsync(m => m.UserId == user.Id, "Topic,Likes", m=>m.OrderByDescending(x =>x.PostDate));

            var cntUserTopics = _unitOfWork.Topic
                .GetAllAsync(m => m.UserId == user.Id).Result.Count();

            var cntUserPosts = userPosts.Count();

            int reputation = 0;

            foreach (var post in userPosts)
            {
                reputation += post.Likes.Count();
            }

            var userProfile = new UserProfileVM()
            {
                User = user,
                CountTopics = cntUserTopics,
                CountPosts = cntUserPosts,
                Reputation = reputation,
                RecentPosts = userPosts.Take(3)
            };
             
            return View(userProfile);
        }
    }
}
