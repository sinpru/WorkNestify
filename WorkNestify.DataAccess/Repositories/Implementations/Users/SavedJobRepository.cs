using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;
using WorkNestify.Models.Models.Users;

namespace WorkNestify.DataAccess.Repositories.Implementations.Users;

public class SavedJobRepository : Repository<SavedJob>, ISavedJob
{
    private readonly ApplicationDbContext _context;
    
    public SavedJobRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(SavedJob job)
    {
        _context.SavedJobs.Update(job);
        await _context.SaveChangesAsync();
    }
}