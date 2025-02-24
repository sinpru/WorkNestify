using WorkNestify.DataAccess.Entities.JobApplications;

namespace WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;

public interface IJobApplicationStatusRepository : IRepository<JobApplicationStatus>
{
    Task UpdateAsync(JobApplicationStatus jobApplicationStatus);
}