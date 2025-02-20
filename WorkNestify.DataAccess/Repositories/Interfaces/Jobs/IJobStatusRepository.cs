using WorkNestify.DataAccess.Entities.Jobs;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

public interface IJobStatusRepository : IRepository<JobStatus>
{
    Task UpdateAsync(JobStatus jobStatus);
}