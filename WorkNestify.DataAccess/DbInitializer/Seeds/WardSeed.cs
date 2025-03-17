

using WorkNestify.Models.Models.Locations;

namespace WorkNestify.DataAccess.DbInitializer.Seeds;

public static class WardSeed
{
    public static List<Ward> GetWards()
    {
        return new List<Ward>
        {
            new Ward { Code = "1A0602", Name = "Phường Dịch Vọng Hậu", DistrictId = 1485 },
            new Ward { Code = "1A0706", Name = "Phường Nhân Chính", DistrictId = 1493 },
            new Ward { Code = "1A0113", Name = "Phường Trúc Bạch", DistrictId = 1484 },
            new Ward { Code = "1A0108", Name = "Phường Ngọc Khánh", DistrictId = 1484 },
            new Ward { Code = "1A0109", Name = "Phường Nguyễn Trung Trực", DistrictId = 1484 },
            new Ward { Code = "1A0201", Name = "Phường Nhật Chiêu", DistrictId = 1497 },
            new Ward { Code = "1A0202", Name = "Phường Xuân La", DistrictId = 1497 },
            new Ward { Code = "1A0217", Name = "Phường Trần Hưng Đạo", DistrictId = 1489 },
            new Ward { Code = "1A0914", Name = "Phường Việt Hưng", DistrictId = 1491 },
            new Ward { Code = "13005", Name = "Phường Mỹ Đình 2", DistrictId = 3440 },
            new Ward { Code = "1A0407", Name = "Phường Láng Thượng", DistrictId = 1486 },
            new Ward { Code = "21402", Name = "Phường 2", DistrictId = 1455 },
            new Ward { Code = "20304", Name = "Phường 4", DistrictId = 1444 },
            new Ward { Code = "21404", Name = "Phường 4", DistrictId = 1455 },
            new Ward { Code = "20709", Name = "Phường Tân Thuận Đông", DistrictId = 1449 },
            new Ward { Code = "20501", Name = "Phường Linh Trung", DistrictId = 1457 },
            new Ward { Code = "20502", Name = "Phường Hiệp Bình Chánh", DistrictId = 1457 },
            new Ward { Code = "20109", Name = "Phường Phạm Ngũ Lão", DistrictId = 1442 },
            new Ward { Code = "20101", Name = "Phường Bến Nghé", DistrictId = 1442 },
            new Ward { Code = "21612", Name = "Phường 17", DistrictId = 1462 },
            new Ward { Code = "2030101", Name = "Phường Thạch Thang", DistrictId = 1494 },
            new Ward { Code = "2030202", Name = "Phường Xuân Hà", DistrictId = 1495 },
            new Ward { Code = "2030301", Name = "Phường Hòa Khánh Bắc", DistrictId = 1498 },
            new Ward { Code = "2030302", Name = "Phường Hòa Hiệp Nam", DistrictId = 1498 },
            new Ward { Code = "2030401", Name = "Phường Mỹ An", DistrictId = 1499 },
            new Ward { Code = "2030402", Name = "Phường Khuê Mỹ", DistrictId = 1499 }
        };
    }
}