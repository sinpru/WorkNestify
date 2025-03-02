using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

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

    public async Task<string> GetProvincesAsync()
    {
        return await PostAsync("master-data/province", new { });
    }

    public async Task<string> GetDistrictsAsync(int provinceId)
    {
        return await PostAsync("master-data/district", new { province_id = provinceId });
    }

    public async Task<string> GetWardsAsync(int districtId)
    {
        return await PostAsync("master-data/ward", new { district_id = districtId });
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