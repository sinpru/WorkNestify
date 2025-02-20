using WorkNestify.DataAccess.Entities.Companies;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Companies;

public interface ICompanySizeRepository : IRepository<CompanySize>
{
    Task UpdateAsync(CompanySize companySize);
}