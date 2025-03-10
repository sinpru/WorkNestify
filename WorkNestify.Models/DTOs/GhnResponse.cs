namespace WorkNestify.Models.DTOs;

public class GhnResponse<T>
{
    public int Code { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}