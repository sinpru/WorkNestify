using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Jobs;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobLevelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobLevelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/JobLevel
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobLevels.ToListAsync());
        }

        // GET: Admin/JobLevel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobLevel = await _context.JobLevels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobLevel == null)
            {
                return NotFound();
            }

            return View(jobLevel);
        }
    }
}
