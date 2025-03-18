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
        
        private async Task PopulateDropdownsAsync(Job? job = null, string? search = null, int? page = null)
        {
            ViewData["JobCategoryId"] = new SelectList(_unitOfWork.JobCategories.GetAllAsync().Result, "Id", "Name");
            ViewData["ProvinceId"] = new SelectList(await _ghnService.GetProvincesAsync(), "Id", "Name");
            ViewData["Level"] = new SelectList(JobLevels.AllLevels);
            ViewData["Status"] = new SelectList(JobStatuses.AllStatuses);
            ViewData["Type"] = new SelectList(JobTypes.AllTypes);
            ViewBag.Search = search;
            ViewBag.Page = page;
        }
    }
}
