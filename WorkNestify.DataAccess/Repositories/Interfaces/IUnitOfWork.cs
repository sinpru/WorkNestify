using WorkNestify.DataAccess.Repositories.Interfaces.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;

namespace WorkNestify.DataAccess.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Companies
    ICompanyRepository Companies { get; }
    ICompanyReviewRepository CompanyReviews { get; }
    ICompanySizeRepository CompanySizes { get; }
    
    // Jobs
    IJobRepository Jobs { get; }
    IJobCategoryRepository JobCategories { get; }
    IJobLevelRepository JobLevels { get; }
    IJobStatusRepository JobStatuses { get; }
    IJobTypeRepository JobTypes { get; }
    
    // JobApplications
    IJobApplicationRepository JobApplications { get; }
    IJobApplicationStatusRepository JobApplicationStatuses { get; }
    
    // Users
    IEmployerRepository Employers { get; }
    IJobSeekerRepository JobSeekers { get; }

    Task<int> SaveAsync();
}