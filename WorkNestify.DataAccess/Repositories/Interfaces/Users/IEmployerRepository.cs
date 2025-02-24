using WorkNestify.DataAccess.Entities.Users;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Users;

public interface IEmployerRepository : IRepository<Employer>
{
    Task UpdateAsync(Employer employer);
}