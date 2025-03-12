using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces.Locations;
using WorkNestify.Models.Models.Locations;

namespace WorkNestify.DataAccess.Repositories.Implementations.Locations;

public class ProvinceRepository : Repository<Province>, IProvinceRepository
{
    private readonly ApplicationDbContext _context;

    public ProvinceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(Province province)
    {
        _context.Update(province);
        await _context.SaveChangesAsync();
    }
}