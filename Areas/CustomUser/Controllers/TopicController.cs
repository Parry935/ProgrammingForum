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
using Microsoft.EntityFrameworkCore;

namespace Forum.Areas.CustomUser.Controllers
{
    [Area("CustomUser")]
    public class TopicController : Controller
    {

        private readonly ApplicationDbContext _db;

        public TopicController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var topicList = await _db.Topic.Where(m => m.CategoryId == id).Include(m=>m.User).ToListAsync();

            var caregoryFromDB = await _db.Category.SingleOrDefaultAsync(m => m.Id == id);

            TopicVM topicVM = new TopicVM()
            {
                Topics = topicList,
                Category = caregoryFromDB
            };

            return View(topicVM);
        }

        [Authorize]
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateTopicVM createTopic = new CreateTopicVM()
            {
                Topic = new Topic(),
                Post = new Post()
            };

            var categoryFromDB = await _db.Category.SingleOrDefaultAsync(m => m.Id == id);

            createTopic.Topic.Category = categoryFromDB;
            createTopic.Topic.CategoryId = categoryFromDB.Id;

            return View(createTopic);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CreateTopicVM createTopic)
        {
            if (!ModelState.IsValid)
            {
                return View(createTopic);
            }


            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            DateTime timeNow = DateTime.Now;

            var categoryFromDB = await _db.Category.SingleOrDefaultAsync(m => m.Id == createTopic.Topic.CategoryId);


            Topic topicToDb = new Topic()
            {
                TopicTittle = createTopic.Topic.TopicTittle,
                TopicDate = timeNow,
                UserId = claim.Value,
                CategoryId = categoryFromDB.Id
            };


            _db.Topic.Add(topicToDb);
            await _db.SaveChangesAsync();



            Post postToDb = new Post()
            {
                CategoryId = categoryFromDB.Id,
                UserId = claim.Value,
                TopicId = topicToDb.Id,
                PostDate = timeNow,
                PostMessage = createTopic.Post.PostMessage   
            };

            _db.Post.Add(postToDb);
            await _db.SaveChangesAsync();


            return RedirectToAction("Index", new { id = createTopic.Topic.CategoryId });
        }


        public async Task<IActionResult> TopicDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topicFromDb = await _db.Topic.SingleOrDefaultAsync(m=>m.Id == id);

            if (topicFromDb == null)
            {
                return NotFound();
            }

            var postList = await _db.Post.Include(m => m.User).Include(m=>m.Topic).Include(m=>m.Category).Where(m=>m.TopicId == topicFromDb.Id).ToListAsync();

            CreatePostVM createPost = new CreatePostVM()
            {
                Posts = postList,
                NewPost = new Post()
                {
                    TopicId = topicFromDb.Id,
                    CategoryId = topicFromDb.CategoryId
                }
            };

            return View(createPost);
        }
    }
}
