using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces;

namespace WorkNestify.DataAccess.Repositories.Implementations;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Update(company);
        await _context.SaveChangesAsync();
    }
}