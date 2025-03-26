using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Companies;

namespace WorkNestify.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-reviews")]
        public async Task<IActionResult> GetReviews(
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                if (page < 1 || pageSize < 1)
                {
                    return BadRequest(new { error = "Page and PageSize must be greater than 0" });
                }

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
                    ).Select(cr => new
                    {
                        id = cr.Id,
                        rating = cr.Rating,
                        content = cr.Content,
                        company = cr.Company != null ? cr.Company.Name : null,
                        logo = cr.Company != null ? cr.Company.Logo : null,
                        createdDate = cr.CreatedDate,
                        modifiedDate = cr.ModifiedDate,
                    }).ToListAsync();

                var response = new
                {
                    reviews = paginatedyReviews,
                    totalReviews,
                    page,
                    pageSize
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while fetching the reviews: " + ex.Message });
            }
        }
    }
}
