using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Services;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class JobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GhnService _ghnService;
        private readonly LocationManager _locationManager;

        public JobController(IUnitOfWork unitOfWork, GhnService ghnService, LocationManager locationManager)
        {
            _unitOfWork = unitOfWork;
            _ghnService = ghnService;
            _locationManager = locationManager;
        }

        // GET: Admin/Job
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var jobsList = _unitOfWork.Jobs
                .GetAllAsync(includeProperties: "Company,JobCategory").Result;
            return Json(new
            {
                data = jobsList.Select(j => new
                {
                    j.Id,
                    j.Title,
                    Company = j.Company?.Name ?? j.Company?.StreetAddress,
                    j.StreetAddress,
                    j.Salary,
                    j.Type,
                    j.Status,
                    JobCategory = j.JobCategory?.Name,
                    CreatedDate = j.CreatedDate.ToString("o") // ISO 8601 for JavaScript
                })
            });
        }

        // GET: Admin/Job/Details/5
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

            return View(job);
        }

        // GET: Admin/Job/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            PopulateDateFields();
            return View();
        }

        // POST: Admin/Job/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,CompanyId,Salary,JobTypeId,JobStatusId,JobLevelId,JobCategoryId,ProvinceId,DistrictId,WardCode,StreetAddress,StartDate,EndDate,Description")]
            Job job)
        {
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

        // GET: Admin/Job/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            await PopulateDropdownsAsync(job);
            PopulateDateFields(job);
            return View(job);
        }

        // POST: Admin/Job/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Title,CompanyId,Salary,Type,Status,Level,JobCategoryId,ProvinceId,DistrictId,WardCode,StreetAddress,StartDate,EndDate,Description")]
            Job job)
        {
            // TODO: Find a way to update the job status to expire when the end date is due
            if (id != job.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
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
                    await PopulateDropdownsAsync();
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
                await PopulateDropdownsAsync();
                PopulateDateFields(job);
                return View(job);
            }
        }

        // GET: Admin/Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(job);
        }

        // POST: Admin/Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id, 
                    includeProperties: "Company,JobCategory,Province,District,Ward");

            if (job == null)
            {
                return NotFound();
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
                await PopulateDropdownsAsync();
                return View(job);
            }
        }

        private async Task PopulateDropdownsAsync(Job? job = null)
        {
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
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
        
        private void PopulateDateFields(Job job = null)
        {
            var currentDate = DateTime.Now;
            var futureDate = currentDate.AddDays(14);
            
            ViewData["EndDate"] = job?.EndDate?.ToString("yyyy-MM-dd") ?? futureDate.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = job?.StartDate?.ToString("yyyy-MM-dd") ?? currentDate.ToString("yyyy-MM-dd");
        }
    }
}