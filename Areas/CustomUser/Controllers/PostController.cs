using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Interfaces.Data;
using Forum.Models;
using Forum.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Areas.CustomUser.Controllers
{

    [Area("CustomUser")]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostsInTopicAndNewPostVM postVM)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);



            DateTime timeNow = DateTime.Now;

            Post postToDb = new Post()
            { 
                UserId = claim.Value,
                TopicId = postVM.NewPost.TopicId,
                CategoryId = postVM.NewPost.CategoryId,
                PostDate = timeNow,
                PostMessage = postVM.NewPost.PostMessage
            };

            _unitOfWork.Post.Insert(postToDb);
            await _unitOfWork.SaveAsync();

            _unitOfWork.Category.addPostCount(postToDb.CategoryId);
            await _unitOfWork.SaveAsync();

            _unitOfWork.Topic.addPostCount(postToDb.TopicId);
            await _unitOfWork.SaveAsync();


            return RedirectToAction("TopicDetails", "Topic", new { id = postVM.NewPost.TopicId});
        }
    }
}
