namespace WorkNestify.DataAccess.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository Companies { get; }
    ICompanyReviewRepository CompanyReviews { get; }
    IJobRepository Jobs { get; }
    IJobApplicationRepository JobApplications { get; }

    Task<int> SaveAsync();
}