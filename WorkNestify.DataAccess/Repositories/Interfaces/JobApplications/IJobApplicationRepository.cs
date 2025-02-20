using WorkNestify.DataAccess.Entities.JobApplications;

namespace WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    Task UpdateAsync(JobApplication jobApplication);
}