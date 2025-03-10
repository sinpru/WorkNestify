using WorkNestify.DataAccess.Entities.Locations;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Locations;

public interface IWardRepository : IRepository<Ward>
{
    Task UpdateAsync(Ward ward);
}