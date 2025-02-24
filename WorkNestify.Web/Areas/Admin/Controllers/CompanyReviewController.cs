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
    public class CompanyReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/CompanyReview
        public async Task<IActionResult> Index()
        {
            var companyReviews = await _unitOfWork.CompanyReviews.GetAllAsync(includeProperties: "Company");
            return View(companyReviews);
        }

        // GET: Admin/CompanyReview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id, includeProperties: "Company");
            
            if (companyReview == null)
            {
                return NotFound();
            }

            return View(companyReview);
        }

        // GET: Admin/CompanyReview/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Address");
            return View();
        }

        // POST: Admin/CompanyReview/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Rating,CompanyId")] CompanyReview companyReview)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.CompanyReviews.AddAsync(companyReview);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Address", companyReview.CompanyId);
            return View(companyReview);
        }

        // GET: Admin/CompanyReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id, includeProperties: "Company");
            
            if (companyReview == null)
            {
                return NotFound();
            }
            
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Address", companyReview.CompanyId);
            return View(companyReview);
        }

        // POST: Admin/CompanyReview/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Rating,CompanyId")] CompanyReview companyReview)
        {
            if (id != companyReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.CompanyReviews.UpdateAsync(companyReview);
                    await _unitOfWork.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CompanyReviewExists(companyReview.Id))
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
            
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Address", companyReview.CompanyId);
            return View(companyReview);
        }

        // GET: Admin/CompanyReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id, includeProperties: "Company");
            
            if (companyReview == null)
            {
                return NotFound();
            }

            return View(companyReview);
        }

        // POST: Admin/CompanyReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyReview = await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id, includeProperties: "Company");
            
            if (companyReview != null)
            {
                _unitOfWork.CompanyReviews.Remove(companyReview);
                await _unitOfWork.SaveAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CompanyReviewExists(int id)
        {
            return await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id) != null;
        }
    }
}
