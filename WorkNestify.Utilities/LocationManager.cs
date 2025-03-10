using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Utilities.Services;

namespace WorkNestify.Utilities;

public class LocationManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly GhnService _ghnService;

    public LocationManager(IUnitOfWork unitOfWork, GhnService ghnService)
    {
        _unitOfWork = unitOfWork;
        _ghnService = ghnService;
    }

    public async Task<bool> EnsureLocationExists(int provinceId, int districtId, string wardCode)
    {
        using (var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync())
        {
            try
            {
                var province = await _unitOfWork.Provinces.GetAsync(p => p.Id == provinceId);
                if (province == null)
                {
                    var newProvinces = await _ghnService.GetProvincesAsync();
                    var foundProvince = newProvinces.FirstOrDefault(p => p.Id == provinceId);
                    if (foundProvince != null)
                    {
                        await _unitOfWork.Provinces.AddAsync(foundProvince);
                        await _unitOfWork.SaveAsync();
                        Console.WriteLine($"Added Province: Id={foundProvince.Id}");
                    }
                    else
                    {
                        Console.WriteLine($"Province {provinceId} not found.");
                        return false;
                    }
                }

                var district = await _unitOfWork.Districts.GetAsync(d => d.Id == districtId);
                if (district == null)
                {
                    var newDistricts = await _ghnService.GetDistrictsAsync(provinceId);
                    var foundDistrict = newDistricts.FirstOrDefault(d => d.Id == districtId);
                    if (foundDistrict != null)
                    {
                        await _unitOfWork.Districts.AddAsync(foundDistrict);
                        await _unitOfWork.SaveAsync();
                        Console.WriteLine($"Added District: Id={foundDistrict.Id}, ProvinceId={foundDistrict.ProvinceId}");
                    }
                    else
                    {
                        Console.WriteLine($"District {districtId} not found.");
                        return false;
                    }
                }

                var ward = await _unitOfWork.Wards.GetAsync(w => w.Code == wardCode);
                if (ward == null)
                {
                    var newWards = await _ghnService.GetWardsAsync(districtId);
                    var foundWard = newWards.FirstOrDefault(w => w.Code == wardCode);
                    if (foundWard != null)
                    {
                        await _unitOfWork.Wards.AddAsync(foundWard);
                        await _unitOfWork.SaveAsync();
                        Console.WriteLine($"Added Ward: Code={foundWard.Code}, DistrictId={foundWard.DistrictId}");
                    }
                    else
                    {
                        Console.WriteLine($"Ward {wardCode} not found.");
                        return false;
                    }
                }

                await _unitOfWork.SaveAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EnsureLocationExists Error: {ex.Message}");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}