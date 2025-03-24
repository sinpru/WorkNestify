using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WorkNestify.Models.DTOs;
using WorkNestify.Models.Models.GhnModels;
using WorkNestify.Models.Models.Locations;

namespace WorkNestify.Services;

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
        var jsonResponse = JsonSerializer.Deserialize<GhnResponse<List<GhnProvince>>>(responseString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return jsonResponse?.Data?.Select(p => new Province
            {
                Id = p.ProvinceID,
                Name = p.ProvinceName
            })
            .OrderBy(p => p.Name)
            .ToList() ?? new List<Province>();
    }

    public async Task<List<District>> GetDistrictsAsync(int provinceId)
    {
        var responseString = await PostAsync("master-data/district", new { province_id = provinceId });
        var jsonResponse = JsonSerializer.Deserialize<GhnResponse<List<GhnDistrict>>>(responseString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return jsonResponse?.Data?.Select(d => new District
            {
                Id = d.DistrictID,
                Name = d.DistrictName,
                ProvinceId = provinceId
            })
            .OrderBy(d => d.Name)
            .ToList() ?? new List<District>();
    }

    public async Task<List<Ward>> GetWardsAsync(int districtId)
    {
        var responseString = await PostAsync("master-data/ward", new { district_id = districtId });
        var jsonResponse = JsonSerializer.Deserialize<GhnResponse<List<GhnWard>>>(responseString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return jsonResponse?.Data?.Select(w => new Ward
            {
                Code = w.WardCode,
                Name = w.WardName,
                DistrictId = districtId
            })
            .OrderBy(w => w.Name)
            .ToList() ?? new List<Ward>();
    }
}