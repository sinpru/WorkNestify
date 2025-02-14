using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.DataAccess.Repositories.Implementations;

public class CompanyReviewRepository : Repository<CompanyReview>, ICompanyReviewRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyReviewRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(CompanyReview companyReview)
    {
        _context.Update(companyReview);
        await _context.SaveChangesAsync();
    }
}