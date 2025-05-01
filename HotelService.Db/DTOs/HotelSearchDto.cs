namespace HotelService.Db.DTOs;

public class HotelSearchDto
{
    public string? SearchText{ get; set; }
    public int? Price { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 4;
}