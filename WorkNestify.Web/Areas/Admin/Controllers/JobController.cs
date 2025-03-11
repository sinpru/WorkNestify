using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Utilities;
using WorkNestify.Utilities.Services;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public async Task<IActionResult> Index()
        {
            var jobs = await _unitOfWork.Jobs
                .GetAllAsync(includeProperties: "Company," +
                                                "JobCategory," +
                                                "JobLevel," +
                                                "JobStatus," +
                                                "JobType");
            return View(jobs);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var jobsList = _unitOfWork.Jobs
                .GetAllAsync(includeProperties: "Company,JobCategory,JobLevel,JobStatus,JobType").Result;
            return Json(new
            {
                data = jobsList.Select(j => new
                {
                    j.Id,
                    j.Title,
                    Company = j.Company?.Name ?? j.Company?.StreetAddress, // Prefer Name if available
                    j.StreetAddress,
                    j.Salary,
                    JobType = j.JobType?.Name,
                    JobStatus = j.JobStatus?.Name,
                    JobCategory = j.JobCategory?.Description,
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
                    includeProperties: "Company,JobCategory,JobLevel,JobStatus,JobType");

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
            return View();
        }

        // POST: Admin/Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Title,CompanyId,Salary,JobTypeId,JobStatusId,JobLevelId,JobCategoryId,ProvinceId,DistrictId,WardCode,StreetAddress,StartDate,EndDate,Description")]
            Job job)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(job);
            }

            // Ensure the input location exists
            bool locationExists =
                await _locationManager.EnsureLocationExists(job.ProvinceId, job.DistrictId, job.WardCode);

            if (!locationExists)
            {
                TempData["Warning"] = "Invalid location data.";
                await PopulateDropdownsAsync();
                return View(job);
            }
            
            job.CreatedDate = DateTime.UtcNow;
            job.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Jobs.AddAsync(job);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
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
                    includeProperties: "Company,JobCategory,JobLevel,JobStatus,JobType");

            if (job == null)
            {
                return NotFound();
            }

            await PopulateDropdownsAsync(job);
            return View(job);
        }

        // POST: Admin/Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id,Title,CompanyId,Salary,JobTypeId,JobStatusId,JobLevelId,JobCategoryId,ProvinceId,DistrictId,WardCode,StreetAddress,StartDate,EndDate,Description")]
            Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(job);
            }
            
            bool locationExists =
                await _locationManager.EnsureLocationExists(job.ProvinceId, job.DistrictId, job.WardCode);

            if (!locationExists)
            {
                TempData["Warning"] = "Invalid location data.";
                await PopulateDropdownsAsync();
                return View(job);
            }
            
            try
            {
                await _unitOfWork.Jobs.UpdateAsync(job);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await JobExists(job.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
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
                    includeProperties: "Company,JobCategory,JobLevel,JobStatus,JobType");

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
                    includeProperties: "Company,JobCategory,JobLevel,JobStatus,JobType");

            if (job != null)
            {
                _unitOfWork.Jobs.Remove(job);
                await _unitOfWork.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> JobExists(int id)
        {
            return await _unitOfWork.Jobs.GetAsync(j => j.Id == id) != null;
        }

        private async Task PopulateDropdownsAsync(Job? job = null)
        {
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
            ViewData["JobCategoryId"] = new SelectList(_unitOfWork.JobCategories.GetAllAsync().Result, "Id", "Name");
            ViewData["JobLevelId"] = new SelectList(_unitOfWork.JobLevels.GetAllAsync().Result, "Id", "Name");
            ViewData["JobStatusId"] = new SelectList(_unitOfWork.JobStatuses.GetAllAsync().Result, "Id", "Name");
            ViewData["JobTypeId"] = new SelectList(_unitOfWork.JobTypes.GetAllAsync().Result, "Id", "Name");
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
    }
}