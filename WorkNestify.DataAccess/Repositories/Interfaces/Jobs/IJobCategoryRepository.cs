using WorkNestify.Models.Models.Jobs;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

public interface IJobCategoryRepository : IRepository<JobCategory>
{
    Task UpdateAsync(JobCategory jobCategory);
}