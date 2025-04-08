using HotelService.Db.Model;

namespace HotelService.Db.DTOs;

public class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }
    public double LogPrice { get; set; } 
    
    public ICollection<ReservationDto> Reservations { get; set; }
}