using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.JobApplications;
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
        public async Task<IActionResult> Index()
        {
            var jobApplicationStatuses = await _unitOfWork.JobApplicationStatuses.GetAllAsync();
            return View(jobApplicationStatuses);
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
