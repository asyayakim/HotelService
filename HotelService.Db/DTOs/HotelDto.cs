namespace HotelService.Db.DTOs;

public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }
    public decimal LogPrice { get; set; } 
}