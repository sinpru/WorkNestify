using WorkNestify.DataAccess.Entities.Companies;

namespace WorkNestify.DataAccess.Repositories.Interfaces;

public interface ICompanyReviewRepository : IRepository<CompanyReview>
{
    Task UpdateAsync(CompanyReview companyReview);
}