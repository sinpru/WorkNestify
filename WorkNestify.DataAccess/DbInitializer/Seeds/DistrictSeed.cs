using WorkNestify.Models.Models.Locations;

namespace WorkNestify.DataAccess.DbInitializer.Seeds;

public static class DistrictSeed
{
    public static List<District> GetDistricts()
    {
        return new List<District>
        {
            new District { Id = 1488, Name = "Quận Hai Bà Trưng", ProvinceId = 201 },
            new District { Id = 1489, Name = "Quận Hoàn Kiếm", ProvinceId = 201 },
            new District { Id = 1486, Name = "Quận Đống Đa", ProvinceId = 201 },
            new District { Id = 1485, Name = "Quận Cầu Giấy", ProvinceId = 201 },
            new District { Id = 1493, Name = "Quận Thanh Xuân", ProvinceId = 201 },
            new District { Id = 1484, Name = "Quận Ba Đình", ProvinceId = 201 },
            new District { Id = 1492, Name = "Quận Tây Hồ", ProvinceId = 201 },
            new District { Id = 1491, Name = "Quận Long Biên", ProvinceId = 201 },
            new District { Id = 3440, Name = "Quận Nam Từ Liêm", ProvinceId = 201 },
            new District { Id = 1455, Name = "Quận Tân Bình", ProvinceId = 202 },
            new District { Id = 1444, Name = "Quận 3", ProvinceId = 202 },
            new District { Id = 1442, Name = "Quận 1", ProvinceId = 202 },
            new District { Id = 1446, Name = "Quận 4", ProvinceId = 202 },
            new District { Id = 1462, Name = "Quận Bình Thạnh", ProvinceId = 202 },
            new District { Id = 1449, Name = "Quận 7", ProvinceId = 202 },
            new District { Id = 1463, Name = "Quận Thủ Đức", ProvinceId = 202 },
            new District { Id = 1526, Name = "Quận Hải Châu", ProvinceId = 203 },
            new District { Id = 1527, Name = "Quận Thanh Khê", ProvinceId = 203 },
            new District { Id = 1530, Name = "Quận Liên Chiểu", ProvinceId = 203 },
            new District { Id = 1529, Name = "Quận Ngũ Hành Sơn", ProvinceId = 203 }
        };
    }
}