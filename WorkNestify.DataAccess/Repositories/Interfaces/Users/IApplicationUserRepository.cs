using WorkNestify.Models.Models.Users;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Users;

public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    Task UpdateAsync(ApplicationUser applicationUser);
}