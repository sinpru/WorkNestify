using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces.Locations;
using WorkNestify.Models.Models.Locations;

namespace WorkNestify.DataAccess.Repositories.Implementations.Locations;

public class DistrictRepository : Repository<District>, IDistrictRepository
{
    private readonly ApplicationDbContext _context;

    public DistrictRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(District district)
    {
        _context.Districts.Update(district);
        await _context.SaveChangesAsync();
    }
}