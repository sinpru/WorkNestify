using WorkNestify.DataAccess.Entities.Jobs;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

public interface IJobRepository : IRepository<Job>
{
    Task UpdateAsync(Job job);
}