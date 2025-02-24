using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Companies;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanySizeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanySizeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CompanySize
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompanySizes.ToListAsync());
        }

        // GET: Admin/CompanySize/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySize = await _context.CompanySizes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companySize == null)
            {
                return NotFound();
            }

            return View(companySize);
        }

        private bool CompanySizeExists(int id)
        {
            return _context.CompanySizes.Any(e => e.Id == id);
        }
    }
}
