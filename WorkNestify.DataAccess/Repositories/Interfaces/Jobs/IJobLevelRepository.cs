using WorkNestify.DataAccess.Entities.Jobs;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

public interface IJobLevelRepository : IRepository<JobLevel>
{
    Task UpdateAsync(JobLevel jobLevel);
}