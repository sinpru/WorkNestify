using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Jobs;

namespace WorkNestify.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredJobs(
            string? search,
            string? category,
            string? location,
            int page = 1,
            int pageSize = 6)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    return BadRequest(new { error = "Page and pageSize must be positive numbers" });
                }

                // Define the filter
                Expression<Func<Job, bool>> filter = j =>
                    j.Status == "Open"
                    && (string.IsNullOrEmpty(search) || j.Title.Contains(search))
                    && (string.IsNullOrEmpty(category) || j.JobCategoryId.ToString() == category)
                    && (string.IsNullOrEmpty(location) || j.StreetAddress.Contains(location));

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
                ).Select(j => new
                {
                    id = j.Id,
                    title = j.Title,
                    companyName = j.Company != null ? j.Company.Name : null,
                    companyLogo = j.Company != null ? j.Company.Logo : null,
                    streetAddress = j.StreetAddress,
                    category = j.JobCategory != null ? j.JobCategory.Name : null,
                    province = j.Province != null ? j.Province.Name : null,
                    district = j.District != null ? j.District.Name : null,
                    ward = j.Ward != null ? j.Ward.Name : null,
                    createdDate = j.CreatedDate,
                    salary = j.Salary,
                    status = j.Status,
                    level = j.Level,
                    type = j.Type
                }).ToListAsync();

                var response = new
                {
                    jobs = paginatedJobs,
                    totalJobs,
                    currentPage = page,
                    pageSize
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while fetching jobs: " + ex.Message });
            }
        }
    }
}