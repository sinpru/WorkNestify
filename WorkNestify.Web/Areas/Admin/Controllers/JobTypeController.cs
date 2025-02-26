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
    public class JobTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/JobType
        public async Task<IActionResult> Index()
        {
            var jobTypes = await _unitOfWork.JobTypes.GetAllAsync();
            return View(jobTypes);
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
