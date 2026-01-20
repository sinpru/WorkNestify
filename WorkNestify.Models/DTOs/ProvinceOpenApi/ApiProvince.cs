namespace WorkNestify.Models.DTOs.ProvinceOpenApi;

public class ApiProvince
{
    public int Code { get; set; }
    public string Name { get; set; }
    public string CodeName { get; set; }
    public string DivisionType { get; set; }
    public int PhoneCode { get; set; }
    public List<ApiWard>? Wards { get; set; }
}