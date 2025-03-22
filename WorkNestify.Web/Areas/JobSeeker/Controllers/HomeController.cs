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

    public async Task<IActionResult> Index(
        string search,
        string category,
        string location,
        int page = 1,
        int pageSize = 6)
    {
        // Populate dropdowns for categories and locations
        await PopulateDropdownsAsync();

        // Define the filter
        Expression<Func<Job, bool>> filter = j =>
            j.Status == "Open"
            && (string.IsNullOrEmpty(search) || j.Title.Contains(search))
            && (string.IsNullOrEmpty(category) ||
                j.JobCategoryId.ToString() == category)
            && (string.IsNullOrEmpty(location) ||
                j.StreetAddress.Contains(location));

        // Define ordering
        Expression<Func<Job, object>>[] orderByDescending = new[]
            { (Expression<Func<Job, object>>)(j => j.CreatedDate) };

        // Get total count
        var totalJobsQuery = _unitOfWork.Jobs.GetAllQueryable(filter: filter);
        var totalJobs = await totalJobsQuery.CountAsync();

        // Get paginated results
        var paginatedJobs = await _unitOfWork.Jobs.GetAllQueryable(
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