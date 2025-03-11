using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanySizeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanySizeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/CompanySize
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var companySizesList = _unitOfWork.CompanySizes
                .GetAllAsync().Result;
            return Json(new
            {
                data = companySizesList.Select(cs => new
                {
                    cs.Id,
                    cs.Name
                })
            });
        }

        // GET: Admin/CompanySize/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySize = await _unitOfWork.CompanySizes.GetAsync(cs => cs.Id == id);
            
            if (companySize == null)
            {
                return NotFound();
            }

            return View(companySize);
        }
    }
}
