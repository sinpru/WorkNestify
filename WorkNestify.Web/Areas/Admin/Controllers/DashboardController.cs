using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Utilities.Constants;

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

    }
}
