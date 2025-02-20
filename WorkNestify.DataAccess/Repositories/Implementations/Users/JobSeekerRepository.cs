using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Users;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;

namespace WorkNestify.DataAccess.Repositories.Implementations.Users;

public class JobSeekerRepository : Repository<JobSeeker>, IJobSeekerRepository
{
    private readonly ApplicationDbContext _context;
    
    public JobSeekerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(JobSeeker jobSeeker)
    {
        _context.Update(jobSeeker);
        await _context.SaveChangesAsync();
    }
}