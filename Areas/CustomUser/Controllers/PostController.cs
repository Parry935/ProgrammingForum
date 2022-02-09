using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Interfaces.Data;
using Forum.Models;
using Forum.Utility;
using Forum.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Areas.CustomUser.Controllers
{

    [Area("CustomUser")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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

            _unitOfWork.Category.IncreasePostCount(postToDb.CategoryId);
            await _unitOfWork.SaveAsync();

            _unitOfWork.Topic.IncreasePostCount(postToDb.TopicId);
            await _unitOfWork.SaveAsync();


            return RedirectToAction("TopicDetails", "Topic", new { id = postVM.NewPost.TopicId});
        }

        public async Task<IActionResult> Edit(int id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var post = await _unitOfWork.Post
                .GetFirstOrDefaultAsync(m => m.Id == id, "User");

            if (post.UserId != claim.Value && !User.IsInRole(ForumRole.Admin))
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post post)
        {
            if (post == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(post);

            _unitOfWork.Post.UpdatePostContent(post.Id,post.PostMessage);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("TopicDetails", "Topic", new { id = post.TopicId });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
                return NotFound();

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var post = await _unitOfWork.Post
                .GetFirstOrDefaultAsync(m => m.Id == id, "User");

            if (post == null)
                return NotFound();

            if (_unitOfWork.Post.CheckIfPostIsFirst(post))
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            if (post.UserId != claim.Value && !User.IsInRole(ForumRole.Admin))
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });

            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Post post)
        {
            if (post == null)
                return NotFound();

            _unitOfWork.Category.DecreasePostCount(post.CategoryId);
            await _unitOfWork.SaveAsync();

            _unitOfWork.Topic.DecreasePostCount(post.TopicId);
            await _unitOfWork.SaveAsync();

            _unitOfWork.Post.RemoveById(post.Id);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("TopicDetails", "Topic", new { id = post.TopicId });
        }

    }
}
