using WorkNestify.DataAccess.Entities.Companies;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Companies;

public interface ICompanyRepository : IRepository<Company>
{
    Task UpdateAsync(Company company);
}