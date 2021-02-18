using Forum.Interfaces.Data;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forum.Areas.CustomUser.Controllers
{

    [Area("CustomUser")]
    [Authorize]
    public class LikeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //API call - Create like
        [HttpPost]
        public async Task<IActionResult> CreateLike(int id)
        {

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var post = await _unitOfWork.Post.GetByIdAsync(id);
            if (post == null)
            {
                return Json(new { success = false });
            }

            Like like = new Like
            {
                PostId = post.Id,
                UserId = claim.Value
            };

            _unitOfWork.Like.Insert(like);
            await _unitOfWork.SaveAsync();

            return Json(new { success = true });
        }

        //API call - Delete like
        [HttpDelete]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var like = await _unitOfWork.Like.GetFirstOrDefaultAsync(m => m.PostId == id && m.UserId == claim.Value);
            if (like == null)
            {
                return Json(new { success = false });
            }

            _unitOfWork.Like.Remove(like);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true });
        }

        // GET - partial view
        public async Task<ActionResult> GetLikes(int id)
        {
            var post = await _unitOfWork.Post.GetFirstOrDefaultAsync(m => m.Id == id, "Likes");

            return PartialView("_LikePartial", post);
        }

    }
}
