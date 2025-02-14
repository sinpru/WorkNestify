using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public ICompanyRepository Companies { get; private set; }
    public ICompanyReviewRepository CompanyReviews { get; private set; }
    public IJobRepository Jobs { get; private set; }
    public IJobApplicationRepository JobApplications { get; private set; }

    public UnitOfWork(ApplicationDbContext context,
        ICompanyRepository companyRepository,
        ICompanyReviewRepository companyReviewRepository,
        IJobRepository jobRepository,
        IJobApplicationRepository jobApplicationRepository)
    {
        _context = context;
        Companies = companyRepository;
        CompanyReviews = companyReviewRepository;
        Jobs = jobRepository;
        JobApplications = jobApplicationRepository;
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}