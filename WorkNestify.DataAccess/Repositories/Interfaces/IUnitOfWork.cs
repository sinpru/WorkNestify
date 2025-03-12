using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Locations;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;

// using WorkNestify.DataAccess.Repositories.Interfaces.Users;

namespace WorkNestify.DataAccess.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ApplicationDbContext Context { get; }
    
    // Companies
    ICompanyRepository Companies { get; }
    ICompanyReviewRepository CompanyReviews { get; }
    
    // Jobs
    IJobRepository Jobs { get; }
    IJobCategoryRepository JobCategories { get; }
    
    // JobApplications
    IJobApplicationRepository JobApplications { get; }
    
    // Locations
    IDistrictRepository Districts { get; }
    IProvinceRepository Provinces { get; }
    IWardRepository Wards { get; }
    
    // Users
    IApplicationUserRepository ApplicationUser { get; }

    Task SaveAsync();
}