using WorkNestify.DataAccess.Entities.Locations;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Locations;

public interface IDistrictRepository : IRepository<District>
{
    Task UpdateAsync(District district);
}