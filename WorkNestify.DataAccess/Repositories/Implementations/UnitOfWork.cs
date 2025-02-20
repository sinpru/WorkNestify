using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.DataAccess.Repositories.Interfaces.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;

namespace WorkNestify.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    // Companies
    public ICompanyRepository Companies { get; private set; }
    public ICompanyReviewRepository CompanyReviews { get; private set; }
    public ICompanySizeRepository CompanySizes { get; private set; }

    // Jobs
    public IJobRepository Jobs { get; private set; }
    public IJobCategoryRepository JobCategories { get; private set; }
    public IJobLevelRepository JobLevels { get; private set; }
    public IJobStatusRepository JobStatuses { get; private set; }
    public IJobTypeRepository JobTypes { get; private set; }

    // JobApplications
    public IJobApplicationRepository JobApplications { get; private set; }
    public IJobApplicationStatusRepository JobApplicationStatuses { get; private set; }

    // Users
    public IEmployerRepository Employers { get; private set; }
    public IJobSeekerRepository JobSeekers { get; private set; }

    public UnitOfWork(ApplicationDbContext context,
        ICompanyRepository companyRepository,
        ICompanyReviewRepository companyReviewRepository,
        ICompanySizeRepository companySizeRepository,
        IJobRepository jobRepository,
        IJobCategoryRepository jobCategoryRepository,
        IJobLevelRepository jobLevelRepository,
        IJobStatusRepository jobStatusRepository,
        IJobTypeRepository jobTypeRepository,
        IJobApplicationRepository jobApplicationRepository,
        IJobApplicationStatusRepository jobApplicationStatusRepository,
        IEmployerRepository employerRepository,
        IJobSeekerRepository jobSeekerRepository)
    {
        _context = context;
        Companies = companyRepository;
        CompanyReviews = companyReviewRepository;
        CompanySizes = companySizeRepository;
        Jobs = jobRepository;
        JobCategories = jobCategoryRepository;
        JobLevels = jobLevelRepository;
        JobStatuses = jobStatusRepository;
        JobTypes = jobTypeRepository;
        JobApplications = jobApplicationRepository;
        JobApplicationStatuses = jobApplicationStatusRepository;
        Employers = employerRepository;
        JobSeekers = jobSeekerRepository;
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