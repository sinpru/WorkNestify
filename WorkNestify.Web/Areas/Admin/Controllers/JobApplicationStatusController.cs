using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobApplicationStatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobApplicationStatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/JobApplicationStatus
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var jobApplicationStatusesList = _unitOfWork.JobApplicationStatuses
                .GetAllAsync().Result;
            return Json(new
            {
                data = jobApplicationStatusesList.Select(jas => new
                {
                    jas.Id,
                    jas.Name
                })
            });
        }

        // GET: Admin/JobApplicationStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplicationStatus = await _unitOfWork.JobApplicationStatuses.GetAsync(jas => jas.Id == id);
            
            if (jobApplicationStatus == null)
            {
                return NotFound();
            }

            return View(jobApplicationStatus);
        }
    }
}
