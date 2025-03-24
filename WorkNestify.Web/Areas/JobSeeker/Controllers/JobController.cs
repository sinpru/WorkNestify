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
            string? search,
            string? category,
            int? location,
            int page = 1,
            int pageSize = 10)
        {
            // Populate dropdowns for categories and locations
            await PopulateDropdownsAsync(null, search);

            // Define the filter
            Expression<Func<Job, bool>> filter = j =>
                j.Status == "Open"
                && (string.IsNullOrEmpty(search) || j.Title.Contains(search))
                && (string.IsNullOrEmpty(category) || j.JobCategoryId.ToString() == category)
                && (!location.HasValue || j.ProvinceId == location);

            // Define ordering
            Expression<Func<Job, object>>[] orderByDescending = new[]
                { (Expression<Func<Job, object>>)(j => j.CreatedDate) };

            // Get total count
            var totalJobsQuery = _unitOfWork.Jobs.GetAllQueryable(filter: filter);
            var totalJobs = await totalJobsQuery.CountAsync();

            // Get paginated results
            var paginatedJobs = await _unitOfWork.Jobs
                .GetAllQueryable(
                    filter: filter,
                    includeProperties: "Company,JobCategory,Province,District,Ward",
                    orderByDescending: orderByDescending,
                    skip: (page - 1) * pageSize,
                    take: pageSize
                ).ToListAsync();

            // Pass data to ViewBag for pagination.js
            ViewBag.TotalJobs = totalJobs;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

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
                    includeProperties: "Company,JobCategory,Province,District,Ward");

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        private async Task PopulateDropdownsAsync(Job? job = null, string? search = null)
        {
            ViewData["JobCategoryId"] = new SelectList(await _unitOfWork.JobCategories.GetAllAsync(), "Id", "Name",
                job?.JobCategoryId);
            ViewData["ProvinceId"] =
                new SelectList(await _ghnService.GetProvincesAsync(), "Id", "Name", job?.ProvinceId);
            ViewData["Level"] = new SelectList(JobLevels.AllLevels.Select(l => new { Value = l, Text = l }), "Value",
                "Text");
            ViewData["Status"] = new SelectList(JobStatuses.AllStatuses.Select(s => new { Value = s, Text = s }),
                "Value", "Text");
            ViewData["Type"] =
                new SelectList(JobTypes.AllTypes.Select(t => new { Value = t, Text = t }), "Value", "Text");
            if (search != null)
            {
                ViewBag.Search = search;
            }
        }
    }
}