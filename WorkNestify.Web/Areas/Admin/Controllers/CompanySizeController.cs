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

        // GET: Admin/CompanySize/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CompanySize/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CompanySize companySize)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companySize);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companySize);
        }

        // GET: Admin/CompanySize/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySize = await _context.CompanySizes.FindAsync(id);
            if (companySize == null)
            {
                return NotFound();
            }
            return View(companySize);
        }

        // POST: Admin/CompanySize/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CompanySize companySize)
        {
            if (id != companySize.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companySize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanySizeExists(companySize.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(companySize);
        }

        // GET: Admin/CompanySize/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/CompanySize/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companySize = await _context.CompanySizes.FindAsync(id);
            if (companySize != null)
            {
                _context.CompanySizes.Remove(companySize);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanySizeExists(int id)
        {
            return _context.CompanySizes.Any(e => e.Id == id);
        }
    }
}
