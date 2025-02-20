using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

namespace WorkNestify.DataAccess.Repositories.Implementations.Jobs;

public class JobLevelRepository : Repository<JobLevel>, IJobLevelRepository
{
    private readonly ApplicationDbContext _context;
    
    public JobLevelRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(JobLevel jobLevel)
    {
        _context.Update(jobLevel);
        await _context.SaveChangesAsync();
    }
}