using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Companies;

namespace WorkNestify.Web.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: JobSeeker/Company
        public async Task<IActionResult> Index(
            string? search,
            int page = 1,
            int pageSize = 12)
        {
            // Get total count
            var totalCompaniesQuery = _unitOfWork.Companies
                .GetAllQueryable(c => string.IsNullOrEmpty(search) || c.Name.Contains(search));
            var totalCompanies = await totalCompaniesQuery.CountAsync();
            
            // Get paginated results
            var paginatedCompanies = await _unitOfWork.Companies
                .GetAllAsync(
                    filter: c => string.IsNullOrEmpty(search) || c.Name.Contains(search),
                    includeProperties: "Province,District,Ward",
                    skip: (page - 1) * pageSize,
                    take: pageSize);
            
            // Pass data to ViewBag for pagination.js
            ViewBag.TotalCompanies = totalCompanies;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            
            return View(paginatedCompanies);
        }

        // GET: JobSeeker/Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies
                .GetAsync(c => c.Id == id,
                    includeProperties: "Province,District,Ward");
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }
    }
}