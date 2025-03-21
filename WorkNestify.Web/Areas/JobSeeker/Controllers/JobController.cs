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
        public async Task<IActionResult> Index(
            string search,
            string category,
            string location,
            int page = 1,
            int pageSize = 10)
        {
            // Populate dropdowns for categories and locations
            await PopulateDropdownsAsync();

            // Fetch featured jobs (e.g., status "Open", ordered by CreatedDate, limit to 6)
            var jobsQuery = _unitOfWork.Jobs.GetAllQueryable(
                filter: j => j.Status == "Open"
                             && (string.IsNullOrEmpty(search) || j.Title.Contains(search))
                             && (string.IsNullOrEmpty(category) || j.JobCategoryId.ToString() == category)
                             && (string.IsNullOrEmpty(location) || j.StreetAddress.Contains(location)),
                includeProperties: "Company,JobCategory,Province,District,Ward"
            );

            // Get total count for pagination
            var totalJobs = await jobsQuery.CountAsync();

            // Apply pagination
            var paginatedJobs = await jobsQuery
                .OrderByDescending(j => j.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Pass data to ViewBag for pagination.js
            ViewBag.TotalJobs = totalJobs;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            ViewBag.Search = search;

            return View(paginatedJobs);
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
            ViewData["JobCategoryId"] = new SelectList(await _unitOfWork.JobCategories.GetAllAsync(), "Id", "Name",
                job?.JobCategoryId);
            ViewData["ProvinceId"] =
                new SelectList(await _ghnService.GetProvincesAsync(), "Id", "Name", job?.ProvinceId);
            ViewData["Level"] = new SelectList(JobLevels.AllLevels);
            ViewData["Status"] = new SelectList(JobStatuses.AllStatuses);
            ViewData["Type"] = new SelectList(JobTypes.AllTypes);
            ViewBag.Search = search;
            ViewBag.Page = page;
        }
    }
}