using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces.Companies;
using WorkNestify.Models.Models.Companies;

namespace WorkNestify.DataAccess.Repositories.Implementations.Companies;

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