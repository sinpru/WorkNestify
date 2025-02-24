using WorkNestify.DataAccess.Entities.Companies;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Companies;

public interface ICompanyReviewRepository : IRepository<CompanyReview>
{
    Task UpdateAsync(CompanyReview companyReview);
}