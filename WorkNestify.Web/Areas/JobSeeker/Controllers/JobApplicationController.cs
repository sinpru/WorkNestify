using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.JobApplications;
using WorkNestify.Models.Models.Users;
using WorkNestify.Services;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    public class JobApplicationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CloudinaryService _cloudinary;

        public JobApplicationController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            CloudinaryService cloudinary)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _cloudinary = cloudinary;
        }

        // GET: JobSeeker/JobApplication
        public async Task<IActionResult> Index(
            string? status = null,
            int page = 1,
            int pageSize = 10)
        {
            ViewData["Status"] = new SelectList(JobApplicationStatuses.AllStatuses);
            ViewBag.CurrentStatus = status;

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                var returnUrl = Url.Action("Index", "JobApplication");
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }
    
            // Define the filter
            Expression<Func<JobApplication, bool>> filter = ja => 
                ja.ApplicationUser.Id == currentUser.Id 
                && (string.IsNullOrEmpty(status) || ja.Status == status);

            // Define ordering (kept as requested)
            Expression<Func<JobApplication, object>>[] orderByDescending = new[]
            { 
                (Expression<Func<JobApplication, object>>)(ja => ja.ApplicationDate) 
            };

            // Get total count
            var totalApplications = await _unitOfWork.JobApplications
                .GetAllQueryable(filter: filter)
                .CountAsync();

            // Get paginated results
            var paginatedApplications = await _unitOfWork.JobApplications
                .GetAllQueryable(
                    filter: filter,
                    includeProperties: "Job,Job.Company,Job.Province,Job.District,Job.Ward",
                    orderByDescending: orderByDescending,
                    skip: (page - 1) * pageSize,
                    take: pageSize
                ).ToListAsync();

            // Pass data to ViewBag for pagination.js
            ViewBag.TotalApplications = totalApplications;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
    
            return View(paginatedApplications);
        }

        // GET: JobSeeker/JobApplication/Create
        public async Task<IActionResult> Create(int? jobId)
        {
            // Pass the JobId to the view via ViewBag
            ViewBag.JobId = jobId;

            // Get the current user; if not logged in, redirect to the login page
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                // Construct the return URL to redirect back to this page after login
                var returnUrl = Url.Action("Create", "JobApplication", new { jobId }, protocol: Request.Scheme);
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            ViewBag.ApplicationUserId = user.Id;

            return View();
        }

        // POST: JobSeeker/JobApplication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ApplicationUserId,JobId")] JobApplication jobApplication,
            IFormFile resumeFile,
            IFormFile coverLetterFile)
        {
            ModelState.Remove("Resume");
            ModelState.Remove("CoverLetter");

            if (!ModelState.IsValid)
            {
                await _unitOfWork.JobApplications.AddAsync(jobApplication);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (coverLetterFile.Length > 0 && resumeFile.Length > 0)
                {
                    string coverLetterUrl = await _cloudinary.UploadDocumentAsync(coverLetterFile);
                    string resumeUrl = await _cloudinary.UploadDocumentAsync(resumeFile);

                    if (string.IsNullOrEmpty(coverLetterUrl))
                    {
                        TempData["Warning"] = "Failed to upload cover letter file";
                        return View("Create", jobApplication);
                    }

                    jobApplication.CoverLetter = coverLetterUrl;

                    if (string.IsNullOrEmpty(resumeUrl))
                    {
                        TempData["Warning"] = "Failed to upload resume file";
                        return View("Create", jobApplication);
                    }

                    jobApplication.Resume = resumeUrl;
                }

                await _unitOfWork.JobApplications.AddAsync(jobApplication);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Successfully apply for job!";
                return RedirectToAction("Details", "Job", new { id = jobApplication.JobId, area = "JobSeeker" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating job application: {ex.Message}";
                return RedirectToAction("Details", "Job", new { id = jobApplication.JobId, area = "JobSeeker" });
            }
        }

        // GET: JobSeeker/JobApplication/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications.GetAsync(ja => ja.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                var returnUrl = Url.Action("Details", "JobApplication", new { id }, protocol: Request.Scheme);
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            if (jobApplication.ApplicationUserId != currentUser.Id)
            {
                return Unauthorized();
            }

            return View(jobApplication);
        }

        // GET: JobSeeker/JobApplication/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications.GetAsync(ja => ja.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                var returnUrl = Url.Action("Edit", "JobApplication", new { id }, protocol: Request.Scheme);
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            if (jobApplication.ApplicationUserId != currentUser.Id)
            {
                return Unauthorized();
            }

            return View(jobApplication);
        }

        // POST: JobSeeker/JobApplication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Resume,CoverLetter,ApplicationUserId,JobId")]
            JobApplication jobApplication)
        {
            if (id != jobApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.JobApplications.UpdateAsync(jobApplication);
                    await _unitOfWork.SaveAsync();
                }
                catch (Exception ex)
                {
                }

                return RedirectToAction(nameof(Index));
            }

            return View(jobApplication);
        }

        // GET: JobSeeker/JobApplication/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications.GetAsync(ja => ja.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                var returnUrl = Url.Action("Delete", "JobApplication", new { id }, protocol: Request.Scheme);
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            if (jobApplication.ApplicationUserId != currentUser.Id)
            {
                return Unauthorized();
            }

            return View(jobApplication);
        }

        // POST: JobSeeker/JobApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _unitOfWork.JobApplications.GetAsync(ja => ja.Id == id);
            if (jobApplication != null)
            {
                _unitOfWork.JobApplications.Remove(jobApplication);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}