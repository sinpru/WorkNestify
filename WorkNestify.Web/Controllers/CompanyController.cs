using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("filter-companies")]
        public async Task<IActionResult> GetCompanyFilters(
            string? search,
            int page = 1,
            int pageSize = 12)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    return BadRequest(new { error = "Page and PageSize must be positive numbers" });
                }
                
                // Get total count
                var totalCompaniesQuery = _unitOfWork.Companies
                    .GetAllQueryable(c => string.IsNullOrEmpty(search) || c.Name.Contains(search));
                var totalCompanies = await totalCompaniesQuery.CountAsync();
            
                // Get paginated results
                var paginatedCompanies = await _unitOfWork.Companies
                    .GetAllQueryable(
                        filter: c => string.IsNullOrEmpty(search) || c.Name.Contains(search),
                        includeProperties: "Province,District,Ward",
                        skip: (page - 1) * pageSize,
                        take: pageSize
                        ).Select(c => new
                    {
                        id = c.Id,
                        name = c.Name,
                        logo = c.Logo,
                        streetAddress = c.StreetAddress,
                        ward = c.Ward != null ? c.Ward.Name : null,
                        district = c.District != null ? c.District.Name : null,
                        province = c.Province != null ? c.Province.Name : null,
                        industry = c.Industry,
                        size = c.Size
                    }).ToListAsync();

                var response = new
                {
                    companies = paginatedCompanies,
                    totalCompanies,
                    currentPage = page,
                    pageSize
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occured while fetching companies: " + ex.Message });
            }
        }
    }
}
