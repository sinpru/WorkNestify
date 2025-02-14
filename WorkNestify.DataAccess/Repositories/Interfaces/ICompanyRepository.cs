using WorkNestify.DataAccess.Entities.Companies;

namespace WorkNestify.DataAccess.Repositories.Interfaces;

public interface ICompanyRepository : IRepository<Company>
{
    Task UpdateAsync(Company company);
}