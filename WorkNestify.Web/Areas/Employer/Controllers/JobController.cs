using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Models.Models.Users;
using WorkNestify.Services;
using WorkNestify.Utilities.Constants;
using SelectList = Microsoft.AspNetCore.Mvc.Rendering.SelectList;

namespace WorkNestify.Web.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = Roles.Employer)]
    public class JobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LocationManager _locationManager;
        private readonly GhnService _ghnService;

        public JobController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            LocationManager locationManager,
            GhnService ghnService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _locationManager = locationManager;
            _ghnService = ghnService;
        }

        // GET: Employer/Job
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            
            if (currentUser.CompanyId == 0)
            {
                return BadRequest(new { error = "User is not associated with a company." });
            }

            try
            {
                var jobsList = _unitOfWork.Jobs
                    .GetAllAsync(j => j.CompanyId == currentUser.CompanyId, includeProperties: "Company,JobCategory")
                    .Result;

                var data = jobsList.Select(j => new
                {
                    j.Id,
                    j.Title,
                    j.StreetAddress,
                    j.Salary,
                    Type = j.Type,
                    Status = j.Status,
                    JobCategory = j.JobCategory?.Name ?? "N/A",
                    CreatedDate = j.CreatedDate.ToString("o") // ISO 8601 for JavaScript
                }).ToList();

                return Json(new { data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error retrieving jobs: {ex.Message}" });
            }
        }

        // GET: Employer/Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs.GetAsync(j => j.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            
            // Verify the job belongs to the current user's company
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || job.CompanyId != currentUser.CompanyId)
            {
                return Unauthorized();
            }

            return View(job);
        }

        // GET: Employer/Job/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            PopulateDateFields();
            return View();
        }

        // POST: Employer/Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Salary,JobTypeId,JobStatusId,JobLevelId,JobCategoryId,ProvinceId,DistrictId,WardCode,StreetAddress,StartDate,EndDate,Description")] 
            Job job)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            if (currentUser.CompanyId == 0)
            {
                ModelState.AddModelError("", "User is not associated with a company.");
                await PopulateDropdownsAsync();
                PopulateDateFields();
                return View(job);
            }
            
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                PopulateDateFields();
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
                    return View(job);
                }
                
                job.CompanyId = (int)currentUser.CompanyId!;
                
                await _unitOfWork.Jobs.AddAsync(job);
                await _unitOfWork.SaveAsync();
                    
                TempData["Success"] = "Job created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error creating job: {ex.Message}";
                await PopulateDropdownsAsync();
                PopulateDateFields();
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
                .GetAsync(j => j.Id == id, includeProperties: "Company,JobCategory");
            if (job == null)
            {
                return NotFound();
            }
            
            // Verify the job belongs to the current user's company
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || job.CompanyId != currentUser.CompanyId)
            {
                return Unauthorized();
            }
            
            await PopulateDropdownsAsync();
            PopulateDateFields();
            return View(job);
        }

        // POST: Employer/Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Title,Salary,JobTypeId,JobStatusId,JobLevelId,JobCategoryId,ProvinceId,DistrictId,WardCode,StreetAddress,StartDate,EndDate,Description")] 
            Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                PopulateDateFields();
                return View(job);
            }
            
            try
            {
                bool locationExists =
                    await _locationManager.EnsureLocationExists(job.ProvinceId, job.DistrictId, job.WardCode);

                if (!locationExists)
                {
                    TempData["Warning"] = "Invalid location data.";
                    await PopulateDropdownsAsync();
                    return View(job);
                }
                    
                job.ModifiedDate = DateTime.UtcNow;
                await _unitOfWork.Jobs.UpdateAsync(job);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error updating job: {ex.Message}";
                await PopulateDropdownsAsync();
                PopulateDateFields();
                return View(job);
            }
                
            TempData["Success"] = "Job updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Employer/Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id, includeProperties: "Company,JobCategory");
            if (job == null)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || job.CompanyId != currentUser.CompanyId)
            {
                return Unauthorized();
            }

            return View(job);
        }

        // POST: Employer/Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _unitOfWork.Jobs.GetAsync(j => j.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || job.CompanyId != currentUser.CompanyId)
            {
                return Unauthorized();
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
            
            ViewData["EndDate"] = job?.EndDate?.ToString("yyyy-MM-ddTHH:mm") ?? futureDate.ToString("yyyy-MM-ddTHH:mm");
            ViewData["StartDate"] = job?.StartDate?.ToString("yyyy-MM-ddTHH:mm") ?? currentDate.ToString("yyyy-MM-ddTHH:mm");
        }
    }
}
