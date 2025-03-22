using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Companies;
using WorkNestify.Models.Models.Users;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
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

        // GET: Admin/CompanyReview
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var reviewsList = _unitOfWork.CompanyReviews
                .GetAllAsync(includeProperties: "Company,ApplicationUser").Result;
            return Json(new
            {
                data = reviewsList.Select(cr => new
                {
                    cr.Id,
                    Company = cr.Company?.Name ?? cr.Company?.StreetAddress,
                    Reviewer = cr.ApplicationUser?.FullName,
                    cr.Rating,
                    CreatedDate = cr.CreatedDate.ToString("o")
                })
            });
        }

        // GET: Admin/CompanyReview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id, includeProperties: "Company,ApplicationUser");
            
            if (companyReview == null)
            {
                return NotFound();
            }

            return View(companyReview);
        }

        // GET: Admin/CompanyReview/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
            return View();
        }

        // POST: Admin/CompanyReview/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewerId,CompanyId,Content,Rating")] CompanyReview companyReview)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
                return View(companyReview);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                companyReview.ApplicationUser = user;
                
                await _unitOfWork.CompanyReviews.AddAsync(companyReview);
                await _unitOfWork.SaveAsync();
                
                TempData["Success"] = "Review added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error creating review: {ex.Message}";
                ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
                return View(companyReview);
            }
        }

        // GET: Admin/CompanyReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id, includeProperties: "Company,ApplicationUser");
            
            if (companyReview == null)
            {
                return NotFound();
            }
            
            ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
            return View(companyReview);
        }

        // POST: Admin/CompanyReview/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReviewerId,CompanyId,Content,Rating")] CompanyReview companyReview)
        {
            if (id != companyReview.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
                return View(companyReview);
            }
            
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (companyReview.ApplicationUser != user)
                {
                    ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
                    TempData["Warning"] = "Review doesn't belong to user.";
                    return View(companyReview);
                }
                
                await _unitOfWork.CompanyReviews.UpdateAsync(companyReview);
                await _unitOfWork.SaveAsync();
                
                TempData["Success"] = "Review updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error editing review: {ex.Message}";
                ViewData["CompanyId"] = new SelectList(_unitOfWork.Companies.GetAllAsync().Result, "Id", "Name");
                return View(companyReview);
            }
        }

        // GET: Admin/CompanyReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id, includeProperties: "Company,ApplicationUser");
            
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
            var companyReview = await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id, includeProperties: "Company,ApplicationUser");
            
            if (companyReview == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.CompanyReviews.Remove(companyReview);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Review deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Warning"] = $"Error deleting review: {ex.Message}";
                return View(companyReview);
            }
        }
    }
}
