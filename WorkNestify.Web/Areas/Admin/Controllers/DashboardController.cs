using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Utilities.Constants;
using Microsoft.EntityFrameworkCore; // For ToListAsync or similar if needed

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: DashboardController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetDashboardData()
        {
            try
            {
                var data = new
                {
                    TotalCompanies = _unitOfWork.Companies.GetAllQueryable().Count(),
                    ActiveJobs = _unitOfWork.Jobs.GetAllQueryable(j => j.Status == JobStatuses.Open).Count(),
                    PendingApplications = _unitOfWork.JobApplications.GetAllQueryable(a => a.Status == JobApplicationStatuses.Pending).Count(),
                    AvgRating = _unitOfWork.CompanyReviews.GetAllQueryable().Any() 
                        ? _unitOfWork.CompanyReviews.GetAllQueryable().Average(r => r.Rating) 
                        : 0.0f, // Handle case with no reviews
                    RecentJobs = _unitOfWork.Jobs.GetAllQueryable(includeProperties: "Company")
                        .OrderByDescending(j => j.CreatedDate)
                        .Take(10)
                        .Select(j => new 
                        { 
                            j.Id, 
                            j.Title, 
                            Company = j.Company != null ? new { j.Company.Name } : new { Name = "N/A" }, 
                            j.Status, 
                            j.CreatedDate 
                        })
                        .ToList(), // Materialize the query
                    RecentPendingApplications = _unitOfWork.JobApplications.GetAllQueryable(includeProperties: "Job,ApplicationUser")
                        .Where(a => a.Status == JobApplicationStatuses.Pending)
                        .OrderByDescending(a => a.ApplicationDate)
                        .Take(10)
                        .Select(a => new 
                        { 
                            a.Id, 
                            Job = a.Job != null ? new { a.Job.Title } : new { Title = "N/A" }, 
                            ApplicationUser = a.ApplicationUser != null ? new { a.ApplicationUser.FullName } : new { FullName = "N/A" }, 
                            a.ApplicationDate 
                        })
                        .ToList(), // Materialize the query
                    RecentReviews = _unitOfWork.CompanyReviews.GetAllQueryable(includeProperties: "Company")
                        .OrderByDescending(r => r.CreatedDate)
                        .Take(10)
                        .Select(r => new 
                        { 
                            r.Id, 
                            Company = r.Company != null ? new { r.Company.Name } : new { Name = "N/A" }, 
                            r.Rating, 
                            r.Content, 
                            r.CreatedDate 
                        })
                        .ToList() // Materialize the query
                };

                return Json(data);
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging mechanism
                return StatusCode(500, new { Error = "An error occurred while fetching dashboard data.", Details = ex.Message });
            }
        }
    }
}