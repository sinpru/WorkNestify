using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Models.Models.Users;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredJobs(
            string? search,
            string? category,
            int? location,
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
                    && (!location.HasValue || j.ProvinceId == location);

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

        [HttpPost("toggle-job")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Employer)]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var job = await _unitOfWork.Jobs.GetAsync(
                    j => j.Id == id,
                    includeProperties: "Company"
                );

                if (job == null)
                {
                    return NotFound(new { success = false, message = "Job not found." });
                }
                
                var currentUser = await _userManager.GetUserAsync(User);
                if (job.CompanyId != currentUser.CompanyId)
                {
                    return Unauthorized(new { success = false, message = "You are not authorized to change the job." });
                }

                if (job.Status == JobStatuses.Pending)
                {
                    return Ok(new { success = false, message = "Cannot toggle pending job." });
                }

                // Toggle status
                job.Status = job.Status == JobStatuses.Open ? JobStatuses.Closed : JobStatuses.Open;
                await _unitOfWork.Jobs.UpdateAsync(job);
                await _unitOfWork.SaveAsync();

                return Ok(new
                {
                    success = true,
                    newStatus = job.Status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"An error occurred: {ex.Message}",
                    details = ex.StackTrace
                });
            }
        }
        
        [HttpPost("accept-job")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AcceptJob(int id)
        {
            try
            {
                var job = await _unitOfWork.Jobs.GetAsync(
                    j => j.Id == id,
                    includeProperties: "Company"
                );

                if (job == null)
                {
                    return NotFound(new { success = false, message = "Job not found." });
                }

                if (job.Status == JobStatuses.Open || job.Status == JobStatuses.Closed)
                {
                    return Ok(new { success = false, message = "Job is already processed." });
                }

                if (job.Status == JobStatuses.Pending || job.Status == JobStatuses.Declined)
                {
                    job.Status = JobStatuses.Open;
                    await _unitOfWork.Jobs.UpdateAsync(job);
                    await _unitOfWork.SaveAsync();
                    
                    return Ok(new
                    {
                        success = true,
                        newStatus = job.Status
                    });
                }
                
                return Ok(new { success = false, message = "Job is not processed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"An error occurred: {ex.Message}",
                    details = ex.StackTrace
                });
            }
        }
        
        [HttpPost("decline-job")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeclineJob(int id)
        {
            try
            {
                var job = await _unitOfWork.Jobs.GetAsync(
                    j => j.Id == id,
                    includeProperties: "Company"
                );

                if (job == null)
                {
                    return NotFound(new { success = false, message = "Job not found." });
                }

                if (job.Status == JobStatuses.Declined)
                {
                    return Ok(new { success = false, message = "Job is already declined." });
                }
                
                job.Status = JobStatuses.Declined;
                await _unitOfWork.Jobs.UpdateAsync(job);
                await _unitOfWork.SaveAsync();

                return Ok(new
                {
                    success = true,
                    newStatus = job.Status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = $"An error occurred: {ex.Message}",
                    details = ex.StackTrace
                });
            }
        }
    }
}