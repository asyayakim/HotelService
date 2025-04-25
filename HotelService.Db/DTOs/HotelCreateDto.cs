namespace HotelService.Db.DTOs;

public class HotelCreateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }
    public double Price { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public bool IsActive { get; set; } = true;
    public List<RoomDto>? Rooms { get; set; } = new List<RoomDto>();
}

public class RoomDto
{
    public string RoomType { get; set; }
    public decimal PricePerNight { get; set; }
    public string ThumbnailRoom { get; set; }
}
