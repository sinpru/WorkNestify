using WorkNestify.DataAccess.Entities.JobApplications;

namespace WorkNestify.DataAccess.Repositories.Interfaces;

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    Task UpdateAsync(JobApplication jobApplication);
}