using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Implementations.Companies;
using WorkNestify.DataAccess.Repositories.Implementations.JobApplications;
using WorkNestify.DataAccess.Repositories.Implementations.Jobs;
using WorkNestify.DataAccess.Repositories.Implementations.Users;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.DataAccess.Repositories.Interfaces.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;

namespace WorkNestify.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public ApplicationDbContext Context => _context;

    // Companies
    public ICompanyRepository Companies { get; private set; }
    public ICompanyReviewRepository CompanyReviews { get; private set; }

    // Jobs
    public IJobRepository Jobs { get; private set; }
    public IJobCategoryRepository JobCategories { get; private set; }

    // JobApplications
    public IJobApplicationRepository JobApplications { get; private set; }
    
    public ISavedJob SavedJobs { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Companies = new CompanyRepository(_context);
        CompanyReviews = new CompanyReviewRepository(_context);
        Jobs = new JobRepository(_context);
        JobCategories = new JobCategoryRepository(_context);
        JobApplications = new JobApplicationRepository(_context);
        SavedJobs = new SavedJobRepository(_context);
    }

    public async Task SaveAsync()
    {
        try
        {
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SaveAsync Error: {ex.Message}");
            // Log entity states for debugging
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                Console.WriteLine($"Entity: {entry.Entity}, State: {entry.State}");
            }
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}