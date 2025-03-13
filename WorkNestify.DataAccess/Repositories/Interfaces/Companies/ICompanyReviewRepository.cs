using WorkNestify.Models.Models.Companies;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Companies;

public interface ICompanyReviewRepository : IRepository<CompanyReview>
{
    Task UpdateAsync(CompanyReview companyReview);
}