using WorkNestify.DataAccess.Entities.Jobs;

namespace WorkNestify.DataAccess.Repositories.Interfaces;

public interface IJobRepository : IRepository<Job>
{
    Task UpdateAsync(Job job);
}