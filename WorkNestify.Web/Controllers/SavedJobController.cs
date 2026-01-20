using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using System.Security.Claims;

namespace WorkNestify.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavedJobController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SavedJobController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListSavedJobs(
            int page = 1, 
            int pageSize = 5)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var query = _unitOfWork.SavedJobs.GetAllQueryable(
                    sj => sj.UserId == userId,
                    includeProperties: "Job,Job.Company"
                ).OrderByDescending(sj => sj.SavedDate);

                var totalSavedJobs = await query.CountAsync();
                var savedJobs = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(sj => new
                    {
                        id = sj.Job.Id,
                        title = sj.Job.Title,
                        companyName = sj.Job.Company.Name,
                        companyLogo = sj.Job.Company.Logo,
                        salary = sj.Job.Salary,
                        provinceCode = sj.Job.ProvinceCode,
                        wardCode = sj.Job.WardCode,
                        streetAddress = sj.Job.StreetAddress,
                        level = sj.Job.Level,
                        type = sj.Job.Type,
                        status = sj.Job.Status,
                        createdDate = sj.Job.CreatedDate,
                        isSaved = true
                    })
                    .ToListAsync();

                return Ok(new { savedJobs, totalSavedJobs });
            }
            catch (Exception ex)
            {
                // Replace with proper logging in production
                Console.WriteLine($"Error in SavedJobController.ListSavedJobs: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving saved jobs.");
            }
        }
    }
}