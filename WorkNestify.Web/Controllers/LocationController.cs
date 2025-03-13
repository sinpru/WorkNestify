using Microsoft.AspNetCore.Mvc;
using WorkNestify.Services;


namespace WorkNestify.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly GhnService _ghnService;

        public LocationController(GhnService ghnService)
        {
            _ghnService = ghnService;
        }

        [HttpGet("districts/{provinceId}")]
        public async Task<IActionResult> GetDistricts(int provinceId)
        {
            var districts = await _ghnService.GetDistrictsAsync(provinceId);
            return Ok(districts);
        }

        [HttpGet("wards/{districtId}")]
        public async Task<IActionResult> GetWards(int districtId)
        {
            var wards = await _ghnService.GetWardsAsync(districtId);
            return Ok(wards);
        }
    }
}
