using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobLevelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobLevelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/JobLevel
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var jobLevelsList = _unitOfWork.JobLevels
                .GetAllAsync().Result;
            return Json(new
            {
                data = jobLevelsList.Select(jl => new
                {
                    jl.Id,
                    jl.Name
                })
            });
        }

        // GET: Admin/JobLevel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobLevel = await _unitOfWork.JobLevels
                .GetAsync(jl => jl.Id == id); 
                
            if (jobLevel == null)
            {
                return NotFound();
            }

            return View(jobLevel);
        }
    }
}
