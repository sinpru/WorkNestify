using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Users;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;

namespace WorkNestify.DataAccess.Repositories.Implementations.Users;

public class EmployerRepository : Repository<Employer>, IEmployerRepository
{
    private readonly ApplicationDbContext _context;
    
    public EmployerRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(Employer employer)
    {
        _context.Update(employer);
        await _context.SaveChangesAsync();
    }
}