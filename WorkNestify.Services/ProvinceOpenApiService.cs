using System.Net.Http.Json;
using System.Text.Json;
using WorkNestify.Models.DTOs.ProvinceOpenApi;

namespace WorkNestify.Services;

public class ProvinceOpenApiService(HttpClient httpClient)
{
    private const string BaseUrl = "https://provinces.open-api.vn/api/v2";

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<ApiProvince>> GetProvincesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<ApiProvince>>($"{BaseUrl}/p/", _jsonOptions) 
               ?? new List<ApiProvince>();
    }

    public async Task<List<ApiWard>> GetWardsByProvinceAsync(int provinceCode)
    {
        var result = await httpClient.GetFromJsonAsync<ApiProvince>($"{BaseUrl}/p/{provinceCode}?depth=2", _jsonOptions);
        
        return result?.Wards ?? new List<ApiWard>();
    }

    public async Task<string?> GetProvinceNameAsync(int code)
    {
        var result = await httpClient.GetFromJsonAsync<ApiProvince>($"{BaseUrl}/p/{code}", _jsonOptions);
        return result?.Name;
    }

    public async Task<string?> GetWardNameAsync(int code)
    {
        var result = await httpClient.GetFromJsonAsync<ApiWard>($"{BaseUrl}/w/{code}", _jsonOptions);
        return result?.Name;
    }
}