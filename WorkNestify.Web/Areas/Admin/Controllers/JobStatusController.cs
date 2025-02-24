using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Jobs;
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
        public async Task<IActionResult> Index()
        {
            var jobStatuses = await _unitOfWork.JobStatuses.GetAllAsync();
            return View(jobStatuses);
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
