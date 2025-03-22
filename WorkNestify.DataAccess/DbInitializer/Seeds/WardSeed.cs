

using WorkNestify.Models.Models.Locations;

namespace WorkNestify.DataAccess.DbInitializer.Seeds;

public static class WardSeed
{
    public static List<Ward> GetWards()
    {
        return new List<Ward>
        {
            new Ward { Code = "1A0602", Name = "Phường Dịch Vọng Hậu", DistrictId = 1485 },
            new Ward { Code = "1A0608", Name = "Phường Yên Hoà", DistrictId = 1485 },
            new Ward { Code = "1A0706", Name = "Phường Nhân Chính", DistrictId = 1493 },
            new Ward { Code = "1A0113", Name = "Phường Trúc Bạch", DistrictId = 1484 },
            new Ward { Code = "1A0108", Name = "Phường Ngọc Khánh", DistrictId = 1484 },
            new Ward { Code = "1A0109", Name = "Phường Nguyễn Trung Trực", DistrictId = 1484 },
            new Ward { Code = "1A0507", Name = "Phường Xuân La", DistrictId = 1492 },
            new Ward { Code = "1A0217", Name = "Phường Trần Hưng Đạo", DistrictId = 1489 },
            new Ward { Code = "1A0218", Name = "Phường Tràng Tiền", DistrictId = 1489 },
            new Ward { Code = "1A0914", Name = "Phường Việt Hưng", DistrictId = 1491 },
            new Ward { Code = "13005", Name = "Phường Mỹ Đình 2", DistrictId = 3440 },
            new Ward { Code = "1A0407", Name = "Phường Láng Thượng", DistrictId = 1486 },
            new Ward { Code = "21402", Name = "Phường 2", DistrictId = 1455 },
            new Ward { Code = "21404", Name = "Phường 4", DistrictId = 1455 },
            new Ward { Code = "20304", Name = "Phường 4", DistrictId = 1444 },
            new Ward { Code = "20709", Name = "Phường Tân Thuận Đông", DistrictId = 1449 },
            new Ward { Code = "21808", Name = "Phường Linh Trung", DistrictId = 1463 },
            new Ward { Code = "21803", Name = "Phường Hiệp Bình Chánh", DistrictId = 1463 },
            new Ward { Code = "20109", Name = "Phường Phạm Ngũ Lão", DistrictId = 1442 },
            new Ward { Code = "20101", Name = "Phường Bến Nghé", DistrictId = 1442 },
            new Ward { Code = "21612", Name = "Phường 17", DistrictId = 1462 },
            new Ward { Code = "40111", Name = "Phường Thạch Thang", DistrictId = 1526 },
            new Ward { Code = "40105", Name = "Phường Hòa Cường Bắc", DistrictId = 1526 },
            new Ward { Code = "40210", Name = "Phường Xuân Hà", DistrictId = 1527 },
            new Ward { Code = "40503", Name = "Phường Hòa Khánh Bắc", DistrictId = 1530 },
            new Ward { Code = "40502", Name = "Phường Hòa Hiệp Nam", DistrictId = 1530 },
            new Ward { Code = "40404", Name = "Phường Mỹ An", DistrictId = 1529 },
            new Ward { Code = "40403", Name = "Phường Khuê Mỹ", DistrictId = 1529 }
        };
    }
}