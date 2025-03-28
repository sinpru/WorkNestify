using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class JobApplicationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CloudinaryService _cloudinary;

        public JobApplicationController(
            IUnitOfWork unitOfWork,
            CloudinaryService cloudinary)
        {
            _unitOfWork = unitOfWork;
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

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                var returnUrl = Url.Action("Index", "JobApplication");
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            // Define the filter
            Expression<Func<JobApplication, bool>> filter = ja =>
                ja.ApplicationUser.Id == userId
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
            // If the user already apply for this job they can't create another one
            var existingJobApplication = await _unitOfWork.JobApplications.GetAsync(ja => ja.JobId == jobId);
            if (existingJobApplication != null && existingJobApplication.Status != JobApplicationStatuses.Withdrawn)
            {
                TempData["Warning"] = "Your job is already in progress.";
                return RedirectToAction("Details", "Job", new { id = jobId, area = "JobSeeker" });
            }

            var currentJob = await _unitOfWork.Jobs.GetAsync(j => j.Id == jobId);
            if (currentJob?.Status != JobStatuses.Open || 
                currentJob.StartDate < DateTime.UtcNow ||
                currentJob.EndDate > DateTime.UtcNow)
            {
                TempData["Warning"] = "You cannot apply for this job right now.";
                return RedirectToAction("Index", "Job", new { area = "JobSeeker" });
            }

            // Pass the JobId to the view via ViewBag
            ViewBag.JobId = jobId;

            // Get the current user; if not logged in, redirect to the login page
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                // Construct the return URL to redirect back to this page after login
                var returnUrl = Url.Action("Create", "JobApplication", new { jobId }, protocol: Request.Scheme);
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            ViewBag.ApplicationUserId = userId;

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
            
            var currentJob = await _unitOfWork.Jobs.GetAsync(j => j.Id == jobApplication.JobId);
            if (currentJob?.Status != JobStatuses.Open || 
                currentJob.StartDate < DateTime.UtcNow ||
                currentJob.EndDate > DateTime.UtcNow)
            {
                TempData["Warning"] = "You cannot apply for this job right now.";
                return RedirectToAction("Index", "JobAppli", new { area = "JobSeeker" });
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                var returnUrl = Url.Action("Edit", "JobApplication", new { id }, protocol: Request.Scheme);
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            if (jobApplication.ApplicationUserId != userId)
            {
                return Unauthorized();
            }

            return View(jobApplication);
        }

        // POST: JobSeeker/JobApplication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ApplicationUserId,JobId")]
            JobApplication jobApplication,
            IFormFile? resumeFile,
            IFormFile? coverLetterFile)
        {
            ModelState.Remove("Resume");
            ModelState.Remove("CoverLetter");
            
            if (id != jobApplication.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(jobApplication);
            }

            try
            {
                var existingJobApplication = await _unitOfWork.JobApplications.GetAsync(ja => ja.Id == id);
                if (existingJobApplication == null)
                {
                    return NotFound();
                }
                
                string existingCoverLetterUrl = existingJobApplication.CoverLetter ?? string.Empty;
                string existingResumeUrl = existingJobApplication.Resume ?? string.Empty;
                
                // Handle Cover Letter Upload
                if (coverLetterFile != null && coverLetterFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingCoverLetterUrl))
                    {
                        bool coverLetterIsDeleted = await _cloudinary.DeleteDocumentAsync(existingCoverLetterUrl);
                        if (!coverLetterIsDeleted)
                        {
                            TempData["Warning"] = "Failed to delete the old cover letter";
                            return View(jobApplication);
                        }
                    }

                    string newCoverLetterUrl = await _cloudinary.UploadDocumentAsync(coverLetterFile);
                    if (string.IsNullOrEmpty(newCoverLetterUrl))
                    {
                        TempData["Warning"] = "Error uploading new cover letter";
                        return View(jobApplication);
                    }

                    existingJobApplication.CoverLetter = newCoverLetterUrl;
                }
                else
                {
                    existingJobApplication.CoverLetter = existingCoverLetterUrl;
                }

                // Handle Resume Upload
                if (resumeFile != null && resumeFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingResumeUrl))
                    {
                        bool resumeIsDeleted = await _cloudinary.DeleteDocumentAsync(existingResumeUrl);
                        if (!resumeIsDeleted)
                        {
                            TempData["Warning"] = "Failed to delete the old resume";
                            return View(jobApplication);
                        }
                    }

                    string newResumeUrl = await _cloudinary.UploadDocumentAsync(resumeFile);
                    if (string.IsNullOrEmpty(newResumeUrl))
                    {
                        TempData["Warning"] = "Error uploading new resume";
                        return View(jobApplication);
                    }

                    existingJobApplication.Resume = newResumeUrl;
                }
                else
                {
                    existingJobApplication.Resume = existingResumeUrl;
                }
                
                existingJobApplication.ModifiedDate = DateTime.UtcNow;
                
                await _unitOfWork.JobApplications.UpdateAsync(jobApplication);
                await _unitOfWork.SaveAsync();
                    
                TempData["Success"] = "Successfully edited job application!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating job application: {ex.Message}";
                return View(jobApplication);
            }
        }
    }
}