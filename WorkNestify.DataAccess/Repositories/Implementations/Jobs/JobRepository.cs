using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;

namespace WorkNestify.DataAccess.Repositories.Implementations.Jobs;

public class JobRepository : Repository<Job>, IJobRepository
{
    private readonly ApplicationDbContext _context;
    
    public JobRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(Job job)
    {
        _context.Update(job);
        await _context.SaveChangesAsync();
    }
}