using WorkNestify.Models.Models.Locations;

namespace WorkNestify.DataAccess.Repositories.Interfaces.Locations;

public interface IProvinceRepository : IRepository<Province>
{
    Task UpdateAsync(Province province);
}