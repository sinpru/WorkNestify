using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WorkNestify.DataAccess.Entities.Locations;
using WorkNestify.Models.DTOs;
using WorkNestify.Models.Models.GhnModels;

namespace WorkNestify.Utilities.Services;

public class GhnService
{
    private readonly HttpClient _httpClient;
    private readonly string? _baseUrl;
    private readonly string? _token;

    public GhnService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["Services:GiaoHangNhanh:BaseUrl"];
        _token = configuration["Services:GiaoHangNhanh:Token"];
    }

    private async Task<string> PostAsync(string endpoint, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Token", _token);

        var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<Province>> GetProvincesAsync()
    {
        var responseString = await PostAsync("master-data/province", new { });
        var jsonResponse = JsonSerializer.Deserialize<GhnResponse<List<GhnProvince>>>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return jsonResponse?.Data?.Select(p => new Province
        {
            Id = p.ProvinceID,
            Name = p.ProvinceName
        }).ToList() ?? new List<Province>();
    }

    public async Task<List<District>> GetDistrictsAsync(int provinceId)
    {
        var responseString = await PostAsync("master-data/district", new { province_id = provinceId });
        var jsonResponse = JsonSerializer.Deserialize<GhnResponse<List<GhnDistrict>>>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        return jsonResponse?.Data?.Select(d => new District
        {
            Id = d.DistrictID,
            Name = d.DistrictName,
            ProvinceId = provinceId
        }).ToList() ?? new List<District>();
    }

    public async Task<List<Ward>> GetWardsAsync(int districtId)
    {
        var responseString = await PostAsync("master-data/ward", new { district_id = districtId });
        var jsonResponse = JsonSerializer.Deserialize<GhnResponse<List<GhnWard>>>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return jsonResponse?.Data?.Select(w => new Ward
        {
            Code = w.WardCode,
            Name = w.WardName,
            DistrictId = districtId
        }).ToList() ?? new List<Ward>();
    }

    public async Task<string> CalculateShippingFeeAsync(int fromDistrict, int toDistrict, int serviceId, int weight, int height, int width, int length)
    {
        var requestData = new
        {
            from_district_id = fromDistrict,
            to_district_id = toDistrict,
            service_id = serviceId,
            weight,
            height,
            width,
            length
        };
        return await PostAsync("v2/shipping-order/fee", requestData);
    }
}