using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Users;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Employer)]
    public class JobApplicationController : Controller
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

        // GET: Employer/JobApplication
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var jobApplicationsList = _unitOfWork.JobApplications
                .GetAllAsync(ja => ja.Job.CompanyId == currentUser.CompanyId,
                    includeProperties: "Job").Result;
            return Json(new
            {
                data = jobApplicationsList.Select(ja => new
                {
                    ja.Id,
                    ApplicationUser = ja.ApplicationUser?.FullName ?? "Not specified",
                    Job = ja.Job?.Title ?? "Not specified",
                    ja.Status,
                    ja.Resume,
                    ja.CoverLetter,
                    ja.ApplicationDate
                })
            });
        }

        // GET: Employer/JobApplication/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _unitOfWork.JobApplications
                .GetAsync(ja => ja.Id == id,
                    includeProperties: "Job,ApplicationUser");
            if (jobApplication == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (jobApplication.Job.CompanyId != currentUser.CompanyId)
            {
                TempData["Warning"] = "You cannot access this job application.";
                return RedirectToAction(nameof(Index));
            }

            return View(jobApplication);
        }
    }
}