using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

namespace WorkNestify.DataAccess.Repositories.Implementations.Jobs;

public class JobStatusRepository : Repository<JobStatus>, IJobStatusRepository
{
    private readonly ApplicationDbContext _context;
    
    public JobStatusRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(JobStatus jobStatus)
    {
        _context.Update(jobStatus);
        await _context.SaveChangesAsync();
    }
}