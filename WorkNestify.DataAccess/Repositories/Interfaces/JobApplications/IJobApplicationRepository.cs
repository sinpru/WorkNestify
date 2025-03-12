using WorkNestify.Models.Models.JobApplications;

namespace WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;

public interface IJobApplicationRepository : IRepository<JobApplication>
{
    Task UpdateAsync(JobApplication jobApplication);
}