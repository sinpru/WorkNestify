using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.JobApplications;
using WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;

namespace WorkNestify.DataAccess.Repositories.Implementations.JobApplications;

public class JobApplicationStatusRepository : Repository<JobApplicationStatus>, IJobApplicationStatusRepository
{
    private readonly ApplicationDbContext _context;
    
    public JobApplicationStatusRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(JobApplicationStatus jobApplicationStatus)
    {
        _context.Update(jobApplicationStatus);
        await _context.SaveChangesAsync();
    }
}