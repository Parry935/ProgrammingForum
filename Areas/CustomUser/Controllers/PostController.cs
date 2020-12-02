using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Data;
using Forum.Models;
using Forum.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Areas.CustomUser.Controllers
{

    [Area("CustomUser")]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PostController(ApplicationDbContext db)
        {
            _db = db;
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostVM postVM)
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

            _db.Post.Add(postToDb);
            await _db.SaveChangesAsync();

            return RedirectToAction("TopicDetails", "Topic", new { id = postVM.NewPost.TopicId});
        }
    }
}
