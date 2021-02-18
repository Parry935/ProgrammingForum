using Forum.Interfaces.Data;
using Forum.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = ForumRole.Admin)]
    public class ManageTopicController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public ManageTopicController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //API call - Delete topic
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var topic = await _unitOfWork.Topic.GetByIdAsync(id);
            if (topic == null)
            {
                return Json(new { success = false });
            }

            _unitOfWork.Category.decreaseTopicAndPostCount(topic.CategoryId, topic.Id);
            await _unitOfWork.SaveAsync();

            _unitOfWork.Topic.Remove(topic);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true});
        }

        //API call - Lock topic
        [HttpPatch]
        public async Task<IActionResult> LockTopic(int id)
        {
            var topic = await _unitOfWork.Topic.GetByIdAsync(id);
            if (topic == null)
            {
                return Json(new { success = false });
            }
            topic.Lock = true;
            await _unitOfWork.SaveAsync();
            return Json(new { success = true });
        }

        //API call - Unlock topic
        [HttpPatch]
        public async Task<IActionResult> UnlockTopic(int id)
        {
            var topic = await _unitOfWork.Topic.GetByIdAsync(id);
            if (topic == null)
            {
                return Json(new { success = false });
            }
            topic.Lock = false;
            await _unitOfWork.SaveAsync();
            return Json(new { success = true });
        }

        //API call - Add award to topic
        [HttpPatch]
        public async Task<IActionResult> AddAwardTopic(int id)
        {
            var topic = await _unitOfWork.Topic.GetByIdAsync(id);
            if (topic == null)
            {
                return Json(new { success = false });
            }
            topic.Awarded = true;
            await _unitOfWork.SaveAsync();
            return Json(new { success = true });
        }

        //API call - Add award to topic
        [HttpPatch]
        public async Task<IActionResult> RemoveAwardTopic(int id)
        {
            var topic = await _unitOfWork.Topic.GetByIdAsync(id);
            if (topic == null)
            {
                return Json(new { success = false });
            }
            topic.Awarded = false;
            await _unitOfWork.SaveAsync();
            return Json(new { success = true });
        }

    }
}
