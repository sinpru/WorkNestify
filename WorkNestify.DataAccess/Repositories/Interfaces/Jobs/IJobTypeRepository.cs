using WorkNestify.DataAccess.Entities.Jobs;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

public interface IJobTypeRepository : IRepository<JobType>
{
    Task UpdateAsync(JobType jobType);
}