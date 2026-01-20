using Microsoft.AspNetCore.Mvc;
using WorkNestify.Services;

namespace WorkNestify.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController(ProvinceOpenApiService openApiService) : ControllerBase
    {
        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            return Ok(await openApiService.GetProvincesAsync());
        }

        [HttpGet("wards/{provinceId}")]
        public async Task<IActionResult> GetWards(int provinceId)
        {
            var wards = await openApiService.GetWardsByProvinceAsync(provinceId);
            return Ok(wards);
        }
    }
}
