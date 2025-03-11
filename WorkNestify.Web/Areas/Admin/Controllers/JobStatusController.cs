using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobStatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobStatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/JobStatus
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var jobStatusesList = _unitOfWork.JobStatuses
                .GetAllAsync().Result;
            return Json(new
            {
                data = jobStatusesList.Select(js => new
                {
                    js.Id,
                    js.Name
                })
            });
        }

        // GET: Admin/JobStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobStatus = await _unitOfWork.JobStatuses.GetAsync(js => js.Id == id);
            
            if (jobStatus == null)
            {
                return NotFound();
            }

            return View(jobStatus);
        }
    }
}
