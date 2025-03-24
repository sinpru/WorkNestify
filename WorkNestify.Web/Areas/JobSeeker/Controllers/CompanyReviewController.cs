using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Companies;
using WorkNestify.Models.Models.Users;

namespace WorkNestify.Web.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    public class CompanyReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyReviewController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: JobSeeker/CompanyReview
        public async Task<IActionResult> Index()
        {
            var companyReviews = await _unitOfWork.CompanyReviews.GetAllAsync();
            return View(companyReviews);
        }

        // GET: JobSeeker/CompanyReview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews
                .GetAsync(cr => cr.Id == id,
                    includeProperties: "Company,ApplicationUser");
            if (companyReview == null)
            {
                return NotFound();
            }

            return View(companyReview);
        }

        // GET: JobSeeker/CompanyReview/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobSeeker/CompanyReview/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Content,Rating,CreatedDate,ModifiedDate,ApplicationUserId,CompanyId")] CompanyReview companyReview)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.CompanyReviews.AddAsync(companyReview);
                await _unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            
            
            return View(companyReview);
        }

        // GET: JobSeeker/CompanyReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews
                .GetAsync(cr => cr.Id == id,
                    includeProperties: "Company,ApplicationUser");
            if (companyReview == null)
            {
                return NotFound();
            }
            
            return View(companyReview);
        }

        // POST: JobSeeker/CompanyReview/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
            [Bind("Id,Content,Rating,CreatedDate,ModifiedDate,ApplicationUserId,CompanyId")] CompanyReview companyReview)
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
                catch (Exception ex)
                {
                    TempData["Warning"] = $"Error creating company review: {ex.Message}";
                    return RedirectToAction("Details", "Company", new { id = companyReview.CompanyId, area = "JobSeeker" });
                }
                return RedirectToAction(nameof(Index));
            }
            return View(companyReview);
        }

        // GET: JobSeeker/CompanyReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews
                .GetAsync(cr => cr.Id == id,
                    includeProperties: "Company,ApplicationUser");
            if (companyReview == null)
            {
                return NotFound();
            }

            return View(companyReview);
        }

        // POST: JobSeeker/CompanyReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyReview = await _unitOfWork.CompanyReviews
                .GetAsync(cr => cr.Id == id);
            if (companyReview != null)
            {
                _unitOfWork.CompanyReviews.Remove(companyReview);
            }

            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
