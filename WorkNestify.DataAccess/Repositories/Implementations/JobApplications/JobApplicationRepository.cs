using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;
using WorkNestify.Models.Models.JobApplications;

namespace WorkNestify.DataAccess.Repositories.Implementations.JobApplications;

public class JobApplicationRepository : Repository<JobApplication>, IJobApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public JobApplicationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(JobApplication jobApplication)
    {
        _context.Update(jobApplication);
        await _context.SaveChangesAsync();
    }
}