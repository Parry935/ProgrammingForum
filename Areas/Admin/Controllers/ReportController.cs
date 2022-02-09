using Forum.Interfaces.Data;
using Forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forum.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReportController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public ReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {

            var reports = await _unitOfWork.Report
                .GetAllAsync(includeProperties: "Post,User");

            return View(reports);
        }

        //API Call - Create report
        [HttpPost]
        public async Task<IActionResult> Create(int id, string reason)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var report = await _unitOfWork.Report
                .GetFirstOrDefaultAsync(m => m.PostId == id && m.UserId == claim.Value);

            if(report != null)
                return Json(new { success = false});

            var reportToDb = new Report()
            {
                PostId = id,
                UserId = claim.Value,
                Reason = reason
            };

            _unitOfWork.Report.Insert(reportToDb);
            await _unitOfWork.SaveAsync();

            return Json(new { success = true });
        }

        //API call - Delete report
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var report = await _unitOfWork.Report
                .GetByIdAsync(id);

            if (report == null)
                return Json(new { success = false });

            _unitOfWork.Report.Remove(report);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true });
        }
    }
}
