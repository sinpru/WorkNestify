using WorkNestify.Models.Models.Locations;

namespace WorkNestify.DataAccess.DbInitializer.Seeds;

public static class ProvinceSeed
{
    public static List<Province> GetProvinces()
    {
        return new List<Province>
        {
            new Province { Id = 201, Name = "Hà Nội" },
            new Province { Id = 202, Name = "Hồ Chí Minh" },
            new Province { Id = 203, Name = "Đà Nẵng" }
        };
    }
}