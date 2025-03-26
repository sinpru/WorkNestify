using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Companies;

namespace WorkNestify.Web.Areas.JobSeeker.Controllers
{
    [Area("JobSeeker")]
    public class CompanyReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyReviewController(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: JobSeeker/CompanyReview
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 6)
        {
            // Get the current user
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                var returnUrl = Url.Action("Index", "CompanyReview", new { area = "JobSeeker" });
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            // Define filter
            Expression<Func<CompanyReview, bool>> filter = cr => cr.ApplicationUserId == currentUserId;

            // Define ordering
            Expression<Func<CompanyReview, object>>[] orderByDescending = new[]
                { (Expression<Func<CompanyReview, object>>)(cr => cr.CreatedDate) };

            // Get total count
            var totalReviewsQuery = _unitOfWork.CompanyReviews.GetAllQueryable(filter: filter);
            var totalReviews = await totalReviewsQuery.CountAsync();
            
            // Get paginated results
            var paginatedyReviews = await _unitOfWork.CompanyReviews
                .GetAllQueryable(
                    filter: filter,
                    includeProperties: "Company,ApplicationUser",
                    orderByDescending: orderByDescending,
                    skip: (page - 1) * pageSize,
                    take: pageSize
                ).ToListAsync();

            // Pass data to ViewBag for pagination.js
            ViewBag.TotalReviews = totalReviews;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            
            return View(paginatedyReviews);
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

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (companyReview.ApplicationUserId != currentUserId)
            {
                return Unauthorized();
            }

            return View(companyReview);
        }

        // GET: JobSeeker/CompanyReview/Create
        public async Task<IActionResult> Create(
            int companyId)
        {
            // Fetch the company and display its info
            var company = await _unitOfWork.Companies.GetAsync(c => c.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            // Pass the objects to view
            ViewBag.CompanyId = companyId;
            ViewBag.Company = company;

            if (User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value == null)
            {
                var returnUrl = Url.Action("Create", "CompanyReview", new { companyId }, protocol: Request.Scheme);
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            return View();
        }

        // POST: JobSeeker/CompanyReview/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Content,ApplicationUserId,CompanyId,Rating")]
            CompanyReview companyReview)
        {
            if (!ModelState.IsValid)
            {
                // If model state is invalid, repopulate ViewBag for the company info
                var company = _unitOfWork.Companies
                    .GetAsync(c => c.Id == companyReview.CompanyId);
                if (company == null)
                {
                    return NotFound();
                }

                ViewBag.CompanyId = companyReview.CompanyId;
                ViewBag.Company = company;

                TempData["Warning"] = "An error occured while trying to create a company review.";
                return View(companyReview);
            }

            try
            {
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (currentUserId == null)
                {
                    var returnUrl = Url.Action("Create", "CompanyReview", new { companyId = companyReview.CompanyId },
                        protocol: Request.Scheme);
                    return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
                }

                companyReview.ApplicationUserId = currentUserId;

                await _unitOfWork.CompanyReviews.AddAsync(companyReview);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "The review was successfully added.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error while adding review: {ex.Message}";
                var company = await _unitOfWork.Companies.GetAsync(c => c.Id == companyReview.CompanyId);
                if (company == null)
                {
                    return NotFound();
                }

                ViewBag.CompanyId = companyReview.CompanyId;
                ViewBag.Company = company;
                return View(companyReview);
            }
        }

        // GET: JobSeeker/CompanyReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyReview = await _unitOfWork.CompanyReviews
                .GetAsync(cr => cr.Id == id, includeProperties: "Company");
            if (companyReview == null)
            {
                return NotFound();
            }

            // Check if the current user owns this review
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                var returnUrl = Url.Action("Edit", "CompanyReview", new { id }, protocol: Request.Scheme);
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
            }

            if (companyReview.ApplicationUserId != currentUserId)
            {
                return Unauthorized();
            }

            // Pass company info to the view
            ViewBag.CompanyId = companyReview.CompanyId;
            ViewBag.Company = companyReview.Company;

            return View(companyReview);
        }

        // POST: JobSeeker/CompanyReview/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Content,Rating,CompanyId,ApplicationUserId")]
            CompanyReview companyReview)
        {
            if (id != companyReview.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var company = await _unitOfWork.Companies.GetAsync(c => c.Id == companyReview.CompanyId);
                if (company == null)
                {
                    return NotFound();
                }

                ViewBag.CompanyId = companyReview.CompanyId;
                ViewBag.Company = company;

                TempData["Warning"] = "An error occurred while trying to update the company review.";
                return View(companyReview);
            }

            try
            {
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (currentUserId == null)
                {
                    var returnUrl = Url.Action("Edit", "CompanyReview", new { id }, protocol: Request.Scheme);
                    return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl });
                }

                // Fetch the existing review to preserve non-editable fields
                var existingReview = await _unitOfWork.CompanyReviews.GetAsync(cr => cr.Id == id);
                if (existingReview == null)
                {
                    return NotFound();
                }

                if (existingReview.ApplicationUserId != currentUserId)
                {
                    return Unauthorized();
                }

                // Update only editable fields
                existingReview.Content = companyReview.Content;
                existingReview.Rating = companyReview.Rating;
                existingReview.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.CompanyReviews.UpdateAsync(existingReview);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "The review was successfully updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error while updating review: {ex.Message}";
                var company = await _unitOfWork.Companies.GetAsync(c => c.Id == companyReview.CompanyId);
                if (company == null)
                {
                    return NotFound();
                }

                ViewBag.CompanyId = companyReview.CompanyId;
                ViewBag.Company = company;
                return View(companyReview);
            }
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