using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Users;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin + "," + Roles.Employer)]
    public class JobApplicationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobApplicationController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("reviewed/{id}")]
        public async Task<IActionResult> JobApplicationReviewed(int id)
        {
            try
            {
                var jobApplication = await _unitOfWork.JobApplications.GetAsync(
                    ja => ja.Id == id,
                    includeProperties: "Job"
                );

                if (jobApplication == null)
                {
                    return NotFound(new { success = false, message = "Job application not found." });
                }
                
                var currentUser = await _userManager.GetUserAsync(User);
                if (jobApplication.Job.CompanyId != currentUser.CompanyId)
                {
                    return Unauthorized(new { success = false, message = "You are not authorized to review this job application." });
                }

                if (jobApplication.Status != JobApplicationStatuses.Pending)
                {
                    return Ok(new { success = false, message = "Job application is processed." });
                }
                
                jobApplication.Status = JobApplicationStatuses.Reviewed;
                await _unitOfWork.JobApplications.UpdateAsync(jobApplication);
                await _unitOfWork.SaveAsync();
                
                return Ok(new
                {
                    success = true,
                    newStatus = jobApplication.Status,
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