using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Locations;
using WorkNestify.DataAccess.Repositories.Interfaces.Locations;

namespace WorkNestify.DataAccess.Repositories.Implementations.Locations;

public class WardRepository : Repository<Ward>, IWardRepository
{
    private readonly ApplicationDbContext _context;

    public WardRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(Ward ward)
    {
        _context.Update(ward);
        await _context.SaveChangesAsync();
    }
}