using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

namespace WorkNestify.DataAccess.Repositories.Implementations.Jobs;

public class JobCategoryRepository : Repository<JobCategory>, IJobCategoryRepository
{
    private readonly ApplicationDbContext _context;
    
    public JobCategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(JobCategory jobCategory)
    {
        _context.Update(jobCategory);
        await _context.SaveChangesAsync();
    }
}