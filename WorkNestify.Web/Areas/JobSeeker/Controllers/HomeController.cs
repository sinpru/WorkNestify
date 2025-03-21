using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
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

    public async Task<IActionResult> Index(
        string search,
        string category,
        string location,
        int page = 1,
        int pageSize = 6)
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

        return View(paginatedJobs);
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
        ViewData["CompanyId"] = new SelectList(await _unitOfWork.Companies.GetAllAsync(), "Id", "Name");
        ViewData["JobCategoryId"] = new SelectList(await _unitOfWork.JobCategories.GetAllAsync(), "Id", "Name");
        ViewData["ProvinceId"] = new SelectList(await _ghnService.GetProvincesAsync(), "Id", "Name");
    }
}