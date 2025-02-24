using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/Company
        public async Task<IActionResult> Index()
        {
            var companies = _unitOfWork.Companies.GetAllAsync();
            return View(await companies);
        }

        // GET: Admin/Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);
            
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Admin/Company/Create
        public IActionResult Create()
        {
            ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
            return View();
        }

        // POST: Admin/Company/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Website,Email,Phone,Address,Description,Logo,Industry,FoundedDate,CreatedDate,ModifiedDate,CompanySizeId")] Company company)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Companies.AddAsync(company);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
            return View(company);
        }

        // GET: Admin/Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);
            
            if (company == null)
            {
                return NotFound();
            }
            
            ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
            return View(company);
        }

        // POST: Admin/Company/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Website,Email,Phone,Address,Description,Logo,Industry,FoundedDate,CreatedDate,ModifiedDate,CompanySizeId")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Companies.UpdateAsync(company);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CompanyExists(company.Id))
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
            
            ViewData["CompanySizeId"] = new SelectList(_unitOfWork.CompanySizes.GetAllAsync().Result, "Id", "Name");
            return View(company);
        }

        // GET: Admin/Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);
            
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Admin/Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == id);
            
            if (company != null)
            {
                _unitOfWork.Companies.Remove(company);
                await _unitOfWork.SaveAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CompanyExists(int id)
        {
            return await _unitOfWork.Companies.GetAsync(c => c.Id == id) != null;
        }
    }
}
