using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

namespace WorkNestify.DataAccess.Repositories.Implementations.Jobs;

public class JobTypeRepository : Repository<JobType>, IJobTypeRepository
{
    private readonly ApplicationDbContext _context;
    
    public JobTypeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(JobType jobType)
    {
        _context.JobTypes.Update(jobType);
        await _context.SaveChangesAsync();
    }
}