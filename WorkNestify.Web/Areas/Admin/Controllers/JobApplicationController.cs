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

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class JobApplicationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CloudinaryService _cloudinary;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobApplicationController(
            IUnitOfWork unitOfWork, 
            CloudinaryService cloudinary,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _cloudinary = cloudinary;
            _userManager = userManager;
        }

        // GET: Admin/JobApplication
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var jobApplicationsList = _unitOfWork.JobApplications
                .GetAllAsync(includeProperties: "Job,ApplicationUser").Result;
            return Json(new
            {
                data = jobApplicationsList.Select(ja => new
                {
                    ja.Id,
                    ApplicationUser = ja.ApplicationUser?.FullName,
                    Job = ja.Job?.Title,
                    ja.Status,
                    ja.ApplicationDate
                })
            });
        }

        // GET: Admin/JobApplication/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications
                .GetAsync(ja => ja.Id == id, includeProperties: "Job,ApplicationUser");

            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // GET: Admin/JobApplication/Create
        public async Task<IActionResult> Create(int? jobId)
        {
            await PopulateDropdownsAsync();
            return View();
        }

        // POST: Admin/JobApplication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("JobId,ApplicationUserId,Status")]
            JobApplication jobApplication,
            IFormFile coverLetterFile, IFormFile resumeFile)
        {
            var currentJob = await _unitOfWork.Jobs.GetAsync(j => j.Id == jobApplication.JobId);
            if (currentJob?.Status != JobStatuses.Open ||
                currentJob.StartDate < DateTime.UtcNow ||
                currentJob.EndDate > DateTime.UtcNow)
            {
                TempData["Warning"] = "You cannot apply for this job right now.";
                return RedirectToAction("Create", "JobApplication", new { jobId = jobApplication.JobId, area = "Admin" });
            }
            
            ModelState.Remove("Resume");
            ModelState.Remove("CoverLetter");

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(jobApplication);
            }

            try
            {
                if (coverLetterFile.Length > 0 && resumeFile.Length > 0)
                {
                    string coverLetterUrl = await _cloudinary.UploadDocumentAsync(coverLetterFile);
                    string resumeUrl = await _cloudinary.UploadDocumentAsync(resumeFile);

                    if (string.IsNullOrEmpty(coverLetterUrl))
                    {
                        TempData["Warning"] = "Error uploading cover letter";
                        await PopulateDropdownsAsync();
                        return View(jobApplication);
                    }

                    jobApplication.CoverLetter = coverLetterUrl;

                    if (string.IsNullOrEmpty(resumeUrl))
                    {
                        TempData["Warning"] = "Error uploading resume";
                        await PopulateDropdownsAsync();
                        return View(jobApplication);
                    }

                    jobApplication.Resume = resumeUrl;
                }
                
                await _unitOfWork.JobApplications.AddAsync(jobApplication);
                await _unitOfWork.SaveAsync();
                
                TempData["Success"] = "Job application added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error creating job application: {ex.Message}";
                await PopulateDropdownsAsync();
                return View(jobApplication);
            }
        }

        // GET: Admin/JobApplication/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications
                .GetAsync(ja => ja.Id == id, includeProperties: "Job,ApplicationUser");

            if (jobApplication == null)
            {
                return NotFound();
            }

            await PopulateDropdownsAsync();

            // Pass existing CoverLetter and Resume URLs to the view via ViewBag
            ViewBag.ExistingCoverLetter = jobApplication.CoverLetter;
            ViewBag.ExistingResume = jobApplication.Resume;

            return View(jobApplication);
        }

        // POST: Admin/JobApplication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,JobId,ApplicationUserId,Status")]
            JobApplication jobApplication,
            IFormFile? coverLetterFile, IFormFile? resumeFile)
        {
            if (id != jobApplication.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Resume");
            ModelState.Remove("CoverLetter");

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                ViewBag.ExistingCoverLetter = jobApplication.CoverLetter;
                ViewBag.ExistingResume = jobApplication.Resume;
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
                                await PopulateDropdownsAsync(jobApplication);
                                ViewBag.ExistingCoverLetter = existingCoverLetterUrl;
                                ViewBag.ExistingResume = existingResumeUrl;
                                return View(jobApplication);
                            }
                        }

                        string newCoverLetterUrl = await _cloudinary.UploadDocumentAsync(coverLetterFile);
                        if (string.IsNullOrEmpty(newCoverLetterUrl))
                        {
                            TempData["Warning"] = "Error uploading new cover letter";
                            await PopulateDropdownsAsync(jobApplication);
                            ViewBag.ExistingCoverLetter = existingCoverLetterUrl;
                            ViewBag.ExistingResume = existingResumeUrl;
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
                                await PopulateDropdownsAsync(jobApplication);
                                ViewBag.ExistingCoverLetter = existingJobApplication.CoverLetter;
                                ViewBag.ExistingResume = existingResumeUrl;
                                return View(jobApplication);
                            }
                        }

                        string newResumeUrl = await _cloudinary.UploadDocumentAsync(resumeFile);
                        if (string.IsNullOrEmpty(newResumeUrl))
                        {
                            TempData["Warning"] = "Error uploading new resume";
                            await PopulateDropdownsAsync(jobApplication);
                            ViewBag.ExistingCoverLetter = existingJobApplication.CoverLetter;
                            ViewBag.ExistingResume = existingResumeUrl;
                            return View(jobApplication);
                        }

                        existingJobApplication.Resume = newResumeUrl;
                    }
                    else
                    {
                        existingJobApplication.Resume = existingResumeUrl;
                    }

                existingJobApplication.ApplicationUserId = jobApplication.ApplicationUserId;
                existingJobApplication.JobId = jobApplication.JobId;
                existingJobApplication.Status = jobApplication.Status;
                existingJobApplication.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.JobApplications.UpdateAsync(existingJobApplication);
                await _unitOfWork.SaveAsync();
                
                TempData["Success"] = "Job application updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Failed to update job application: {ex.Message}";
                await PopulateDropdownsAsync(jobApplication);
                ViewBag.ExistingCoverLetter = jobApplication.CoverLetter;
                ViewBag.ExistingResume = jobApplication.Resume;
                return View(jobApplication);
            }
        }

        // GET: Admin/JobApplication/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications
                .GetAsync(ja => ja.Id == id, includeProperties: "Job,ApplicationUser");

            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: Admin/JobApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _unitOfWork.JobApplications
                .GetAsync(ja => ja.Id == id, includeProperties: "Job,ApplicationUser");

            if (jobApplication == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.JobApplications.Remove(jobApplication);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Failed to delete job application: {ex.Message}";
                return View(jobApplication);
            }
        }

        private async Task PopulateDropdownsAsync(JobApplication? jobApplication = null)
        {
            ViewData["JobId"] = new SelectList(_unitOfWork.Jobs.GetAllAsync().Result, "Id", "Title", jobApplication?.JobId);
            ViewData["Status"] = new SelectList(JobApplicationStatuses.AllStatuses);
            ViewData["ApplicationUserId"] =
                new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName", jobApplication?.ApplicationUserId);
        }
    }
}