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
using Microsoft.EntityFrameworkCore;

namespace Forum.Areas.CustomUser.Controllers
{
    [Area("CustomUser")]
    public class TopicController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public TopicController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var topicList = await _unitOfWork.Topic.GetAllAsync(m => m.CategoryId == id, "User");

            var caregoryFromDB = await _unitOfWork.Category.GetFirstOrDefaultAsync(m => m.Id == id);

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

            var caregoryFromDB = await _unitOfWork.Category.GetFirstOrDefaultAsync(m => m.Id == id);

            createTopic.Topic.Category = caregoryFromDB;
            createTopic.Topic.CategoryId = caregoryFromDB.Id;

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

            var categoryFromDB = await _unitOfWork.Category.GetFirstOrDefaultAsync(m => m.Id == createTopic.Topic.CategoryId);


            Topic topicToDb = new Topic()
            {
                TopicTittle = createTopic.Topic.TopicTittle,
                TopicDate = timeNow,
                UserId = claim.Value,
                CategoryId = categoryFromDB.Id
            };


            _unitOfWork.Topic.Insert(topicToDb);
            await _unitOfWork.SaveAsync();

            Post postToDb = new Post()
            {
                CategoryId = categoryFromDB.Id,
                UserId = claim.Value,
                TopicId = topicToDb.Id,
                PostDate = timeNow,
                PostMessage = createTopic.Post.PostMessage   
            };

            _unitOfWork.Post.Insert(postToDb);
            await _unitOfWork.SaveAsync();


            return RedirectToAction("Index", new { id = createTopic.Topic.CategoryId });
        }


        public async Task<IActionResult> TopicDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topicFromDb = await _unitOfWork.Topic.GetFirstOrDefaultAsync(m => m.Id == id);

            if (topicFromDb == null)
            {
                return NotFound();
            }

            var postList = await _unitOfWork.Post.GetAllAsync(m => m.TopicId == topicFromDb.Id, "User,Topic,Category");

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
