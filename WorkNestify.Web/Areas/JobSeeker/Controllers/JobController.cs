using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Services;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    public class JobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GhnService _ghnService;

        public JobController(
            IUnitOfWork unitOfWork, 
            GhnService ghnService)
        {
            _unitOfWork = unitOfWork;
            _ghnService = ghnService;
        }

        // GET: JobSeeker/Job
        public async Task<IActionResult> Index(string search, string category, string location, int page = 1)
        {
            const int pageSize = 9;
            var jobsQuery = _unitOfWork.Jobs.GetAllQueryable(
                filter: j => j.Status == "Open"
                             && (string.IsNullOrEmpty(search) || j.Title.Contains(search))
                             && (string.IsNullOrEmpty(category) || j.JobCategoryId.ToString() == category)
                             && (string.IsNullOrEmpty(location) || j.StreetAddress.Contains(location)),
                includeProperties: "Company,JobCategory",
                orderByDescending: new[] { (Expression<Func<Job, object>>)(j => j.CreatedDate) },
                skip: (page - 1) * pageSize,
                take: pageSize
            );
            var totalJobs = await jobsQuery.CountAsync();
            var jobs = await jobsQuery.ToListAsync();

            await PopulateDropdownsAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalJobs / pageSize);
            ViewBag.Search = search;
            ViewBag.Page = page;

            return View(jobs);
        }

        // GET: JobSeeker/Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _unitOfWork.Jobs
                .GetAsync(j => j.Id == id,
                    includeProperties: "Company,JobCategory");
            
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: JobSeeker/Job/Create
        public IActionResult Create()
        {
            PopulateDropdownsAsync();
            return View();
        }

        // POST: JobSeeker/Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Location,Salary,StartDate,EndDate,CreatedDate,ModifiedDate,JobTypeId,JobStatusId,JobLevelId,JobCategoryId,CompanyId")] Job job)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Jobs.AddAsync(job);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            
            PopulateDropdownsAsync();
            return View(job);
        }

        // GET: JobSeeker/Job/Edit/5
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
            
            PopulateDropdownsAsync();
            return View(job);
        }

        // POST: JobSeeker/Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,Salary,StartDate,EndDate,CreatedDate,ModifiedDate,JobTypeId,JobStatusId,JobLevelId,JobCategoryId,CompanyId")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            
            PopulateDropdownsAsync();
            return View(job);
        }

        // GET: JobSeeker/Job/Delete/5
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

        // POST: JobSeeker/Job/Delete/5
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
        
        // GET: JobSeeker/Job/Search?query=developer
        public async Task<IActionResult> Search(string? query)
        {
            var jobs = await _unitOfWork.Jobs
                .GetAllAsync(j => string.IsNullOrEmpty(query) || 
                                  j.Title.Contains(query) ||
                                  j.Description.Contains(query));
            
            return View(jobs);
        }

        private async Task<bool> JobExists(int id)
        {
            return await _unitOfWork.Jobs.GetAsync(j => j.Id == id) != null;
        }
        
        private async Task PopulateDropdownsAsync(Job? job = null, string? search = null, int? page = null)
        {
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Address", job?.CompanyId);
            ViewData["JobCategoryId"] = new SelectList(_unitOfWork.JobCategories.GetAllAsync().Result, "Id", "Name", job?.JobCategoryId);
            ViewData["ProvinceId"] = new SelectList(await _ghnService.GetProvincesAsync(), "Id", "Name");
            ViewData["Level"] = new SelectList(JobLevels.AllLevels);
            ViewData["Status"] = new SelectList(JobStatuses.AllStatuses);
            ViewData["Type"] = new SelectList(JobTypes.AllTypes);
            ViewBag.Search = search;
            ViewBag.Page = page;
        }
    }
}
