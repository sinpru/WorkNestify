using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces.Companies;

namespace WorkNestify.DataAccess.Repositories.Implementations.Companies;

public class CompanySizeRepository : Repository<CompanySize>, ICompanySizeRepository
{
    private readonly ApplicationDbContext _context;
    
    public CompanySizeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(CompanySize companySize)
    {
        _context.Update(companySize);
        await _context.SaveChangesAsync();
    }
}