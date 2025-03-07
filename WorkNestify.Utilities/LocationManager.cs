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
    
    public async Task<bool> EnsureLocationExists(int provinceId, int districtId, int wardId)
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
            }
            else
            {
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
            }
            else
            {
                return false;
            }
        }

        var ward = await _unitOfWork.Wards.GetAsync(w => w.Id == wardId);
        if (ward == null)
        {
            var newWards = await _ghnService.GetWardsAsync(districtId);
            var foundWard = newWards.FirstOrDefault(w => w.Id == wardId);
            if (foundWard != null)
            {
                await _unitOfWork.Wards.AddAsync(foundWard);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}