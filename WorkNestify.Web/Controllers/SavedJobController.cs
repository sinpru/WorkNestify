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
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var query = _unitOfWork.SavedJobs.GetAllQueryable(
                    sj => sj.UserId == userId,
                    includeProperties: "Job,Job.Company,Job.Province,Job.Ward,Job.District"
                ).OrderByDescending(sj => sj.SavedDate);

                var totalSavedJobs = await query.CountAsync();
                var savedJobs = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(sj => new
                    {
                        Id = sj.Job.Id,
                        Title = sj.Job.Title,
                        CompanyName = sj.Job.Company.Name,
                        CompanyLogo = sj.Job.Company.Logo,
                        Salary = sj.Job.Salary,
                        Province = sj.Job.Province != null ? sj.Job.Province.Name : null,
                        Ward = sj.Job.Ward != null ? sj.Job.Ward.Name : null,
                        District = sj.Job.District != null ? sj.Job.District.Name : null,
                        StreetAddress = sj.Job.StreetAddress,
                        Level = sj.Job.Level,
                        Type = sj.Job.Type,
                        Status = sj.Job.Status,
                        CreatedDate = sj.Job.CreatedDate,
                        IsSaved = true
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