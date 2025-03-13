using WorkNestify.Models.Models.Locations;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Locations;

public interface IDistrictRepository : IRepository<District>
{
    Task UpdateAsync(District district);
}