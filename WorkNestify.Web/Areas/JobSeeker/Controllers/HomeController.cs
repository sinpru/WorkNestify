using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Models.ViewModels;
using WorkNestify.Services;

namespace WorkNestify.Web.Areas.JobSeeker.Controllers;

[Area("JobSeeker")]
public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GhnService _ghnService;

    public HomeController(
        IUnitOfWork unitOfWork,
        GhnService ghnService)
    {
        _unitOfWork = unitOfWork;
        _ghnService = ghnService;
    }
    
    public async Task<IActionResult> Index(string search, string category, string location)
    {
        // Populate dropdowns for categories and locations
        await PopulateDropdownsAsync();

        // Fetch featured jobs (e.g., status "Open", ordered by CreatedDate, limit to 6)
        var featuredJobsQuery = _unitOfWork.Jobs.GetAllQueryable(
            filter: j => j.Status == "Open"
                         && (string.IsNullOrEmpty(search) || j.Title.Contains(search))
                         && (string.IsNullOrEmpty(category) || j.JobCategoryId.ToString() == category)
                         && (string.IsNullOrEmpty(location) || j.StreetAddress.Contains(location)),
            includeProperties: "Company,JobCategory",
            orderByDescending: new[] { (Expression<Func<Job, object>>)(j => j.CreatedDate) }, // Explicit cast
            take: 6
        );
        var featuredJobs = await featuredJobsQuery.ToListAsync();

        return View(featuredJobs); // Pass the featured jobs to the view
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    private async Task PopulateDropdownsAsync()
    {
        ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
        ViewData["JobCategoryId"] = new SelectList(_unitOfWork.JobCategories.GetAllAsync().Result, "Id", "Name");
        ViewData["ProvinceId"] = new SelectList(await _ghnService.GetProvincesAsync(), "Id", "Name");
    }
}