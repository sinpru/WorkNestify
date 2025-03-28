using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Users;

namespace WorkNestify.Web.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    [Authorize]
    public class SavedJobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SavedJobController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: JobSeeker/SavedJob
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 5)
        {
            // Get current user
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Define ordering
            Expression<Func<SavedJob, object>>[] orderByDescending = new[]
                { (Expression<Func<SavedJob, object>>)(sj => sj.SavedDate) };

            // Get total count
            var totalSavedJobsQuery = _unitOfWork.SavedJobs.GetAllQueryable(filter: sj => sj.UserId == userId);
            var totalSavedJobs = await totalSavedJobsQuery.CountAsync();

            // Get paginated results
            var paginatedSavedJobs = await _unitOfWork.SavedJobs
                .GetAllQueryable(
                    filter: sj => sj.UserId == userId,
                    includeProperties: "User,Job,Job.Company,Job.Province,Job.District,Job.Ward",
                    orderBy: orderByDescending,
                    skip: (page - 1) * pageSize,
                    take: pageSize
                ).ToListAsync();

            // Pass data to ViewBag for pagination.js
            ViewBag.TotalSavedJobs = totalSavedJobs;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            return View(paginatedSavedJobs);
        }

        [HttpPost]
        public async Task<IActionResult> SaveJob(int jobId)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var jobExists = await _unitOfWork.Jobs.GetAsync(j => j.Id == jobId);
                if (jobExists == null)
                {
                    return Json(new { success = false, message = "Job does not exist." });
                }

                var existingSave = await _unitOfWork.SavedJobs.GetAsync(sj => sj.UserId == userId && sj.JobId == jobId);

                if (existingSave != null)
                {
                    _unitOfWork.SavedJobs.Remove(existingSave);
                    await _unitOfWork.SaveAsync();
                    return Json(new { success = true, saved = false });
                }

                var savedJob = new SavedJob
                {
                    UserId = userId,
                    JobId = jobId
                };

                await _unitOfWork.SavedJobs.AddAsync(savedJob);
                await _unitOfWork.SaveAsync();

                return Json(new { success = true, saved = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "An error occured while saving the job: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> IsJobSaved(int jobId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isSaved = await _unitOfWork.SavedJobs.CountAsync(sj => sj.UserId == userId && sj.JobId == jobId) > 0;
            return Json(new { saved = isSaved });
        }
    }
}