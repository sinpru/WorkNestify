using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Models.Models.Users;
using WorkNestify.Services;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Employer)]
    public class JobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GhnService _ghnService;
        private readonly LocationManager _locationManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobController(
            IUnitOfWork unitOfWork,
            GhnService ghnService,
            LocationManager locationManager,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _ghnService = ghnService;
            _locationManager = locationManager;
            _userManager = userManager;
        }

        // GET: Employer/Job
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            
            var jobsList = _unitOfWork.Jobs
                .GetAllAsync(j => j.CompanyId == currentUser.CompanyId,
                    includeProperties: "JobCategory").Result;
            return Json(new
            {
                data = jobsList.Select(j => new
                {
                    j.Id,
                    j.Title,
                    j.StreetAddress,
                    j.Salary,
                    j.Type,
                    j.Status,
                    j.Level,
                    JobCategory = j.JobCategory?.Name ?? "Not specified",
                    CreatedDate = j.CreatedDate.ToString("o") // ISO 8601 for JavaScript
                })
            });
        }
        
        // GET: Employer/Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id,
                    includeProperties: "Company,JobCategory,Province,District,Ward");
            if (job == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Job", new { area = "JobSeeker", id = job.Id });
        }

        // GET: Employer/Job/Create
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.CompanyId = currentUser?.CompanyId;
            await PopulateDropdownsAsync();
            PopulateDateFields();
            return View();
        }

        // POST: Employer/Job/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Description,StreetAddress,Salary,StartDate,EndDate,Type,Status,Level,JobCategoryId,CompanyId,ProvinceId,DistrictId,WardCode")] 
            Job job)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (job.CompanyId != currentUser.CompanyId)
            {
                TempData["Warning"] = "You are not authorized to access this page.";
                return RedirectToAction(nameof(Index));
            }

            if (job.EndDate < job.StartDate || job.StartDate > job.EndDate || job.EndDate < DateTime.UtcNow)
            {
                TempData["Warning"] = "Dates are invalid.";
                return View(job);
            }
            
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                PopulateDateFields(job);
                return View(job);
            }

            try
            {
                // Ensure the input location exists
                bool locationExists =
                    await _locationManager.EnsureLocationExists(job.ProvinceId, job.DistrictId, job.WardCode);

                if (!locationExists)
                {
                    TempData["Warning"] = "Invalid location data.";
                    await PopulateDropdownsAsync();
                    PopulateDateFields(job);
                    return View(job);
                }

                TempData["Success"] = "Job created successfully.";
                await _unitOfWork.Jobs.AddAsync(job);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error creating job: {ex.Message}";
                await PopulateDropdownsAsync();
                PopulateDateFields(job);
                return View(job);
            }
        }

        // GET: Employer/Job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id,
                    includeProperties: "JobCategory,Province,District,Ward");
            if (job == null)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (job.CompanyId != currentUser.CompanyId)
            {
                TempData["Warning"] = "You cannot edit this job.";
                return RedirectToAction(nameof(Index));
            }
            
            await PopulateDropdownsAsync(job);
            PopulateDateFields(job);
            return View(job);
        }

        // POST: Employer/Job/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Title,Description,StreetAddress,Salary,StartDate,EndDate,Type,Status,Level,JobCategoryId,CompanyId,ProvinceId,DistrictId,WardCode")] 
            Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (job.CompanyId != currentUser.CompanyId)
            {
                TempData["Warning"] = "You cannot edit this job.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(job);
                PopulateDateFields(job);
                return View(job);
            }

            try
            {
                bool locationExists =
                    await _locationManager.EnsureLocationExists(job.ProvinceId, job.DistrictId, job.WardCode);

                if (!locationExists)
                {
                    TempData["Warning"] = "Invalid location data.";
                    await PopulateDropdownsAsync(job);
                    PopulateDateFields(job);
                    return View(job);
                }

                job.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.Jobs.UpdateAsync(job);
                await _unitOfWork.SaveAsync();
                
                TempData["Success"] = "Job updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error updating job: {ex.Message}";
                await PopulateDropdownsAsync(job);
                PopulateDateFields(job);
                return View(job);
            }
        }

        // GET: Employer/Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id,
                    includeProperties: "JobCategory,Province,District,Ward");
            if (job == null)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (job.CompanyId != currentUser.CompanyId)
            {
                TempData["Warning"] = "You cannot edit this job.";
                return RedirectToAction(nameof(Index));
            }

            return View(job);
        }

        // POST: Employer/Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id,
                    includeProperties: "JobCategory,Province,District,Ward");
            
            if (job == null)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (job.CompanyId != currentUser.CompanyId)
            {
                TempData["Warning"] = "You cannot edit this job.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _unitOfWork.Jobs.Remove(job);
                await _unitOfWork.SaveAsync();
                
                TempData["Success"] = "Job deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error deleting job: {ex.Message}";
                await PopulateDropdownsAsync(job);
                return View(job);
            }
        }
        
        private async Task PopulateDropdownsAsync(Job? job = null)
        {
            ViewData["JobCategoryId"] = new SelectList(_unitOfWork.JobCategories.GetAllAsync().Result, "Id", "Name");
            ViewData["Level"] = new SelectList(JobLevels.AllLevels);
            ViewData["Status"] = new SelectList(JobStatuses.AllStatuses);
            ViewData["Type"] = new SelectList(JobTypes.AllTypes);
            ViewData["ProvinceId"] = new SelectList(await _ghnService.GetProvincesAsync(), "Id", "Name");

            if (job != null && job.ProvinceId != 0)
            {
                ViewData["DistrictId"] =
                    new SelectList(await _ghnService.GetDistrictsAsync(job.ProvinceId), "Id", "Name");
            }
            else
            {
                ViewData["DistrictId"] = new SelectList(Enumerable.Empty<object>(), "Id", "Name");
            }

            if (job != null && job.DistrictId != 0)
            {
                ViewData["WardCode"] = new SelectList(await _ghnService.GetWardsAsync(job.DistrictId), "Code", "Name");
            }
            else
            {
                ViewData["WardCode"] = new SelectList(Enumerable.Empty<object>(), "Code", "Name");
            }
        }
        
        private void PopulateDateFields(Job? job = null)
        {
            var currentDate = DateTime.Now;
            var futureDate = currentDate.AddDays(14);
            
            ViewData["EndDate"] = job?.EndDate?.ToString("yyyy-MM-dd") ?? futureDate.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = job?.StartDate?.ToString("yyyy-MM-dd") ?? currentDate.ToString("yyyy-MM-dd");
        }
    }
}
