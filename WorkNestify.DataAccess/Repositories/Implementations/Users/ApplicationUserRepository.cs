using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;
using WorkNestify.Models.Models.Users;

namespace WorkNestify.DataAccess.Repositories.Implementations.Users;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private readonly ApplicationDbContext _context;

    public ApplicationUserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(ApplicationUser applicationUser)
    {
        _context.Update(applicationUser);
        await _context.SaveChangesAsync();
    }
}