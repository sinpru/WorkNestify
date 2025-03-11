using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/JobType
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var jobTypesList = _unitOfWork.JobTypes
                .GetAllAsync().Result;
            return Json(new
            {
                data = jobTypesList.Select(jt => new
                {
                    jt.Id,
                    jt.Name
                })
            });
        }

        // GET: Admin/JobType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _unitOfWork.JobTypes.GetAsync(jt => jt.Id == id);
            
            if (jobType == null)
            {
                return NotFound();
            }

            return View(jobType);
        }
    }
}
