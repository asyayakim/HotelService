using System.ComponentModel.DataAnnotations;

namespace HotelService.Db.DTOs;

public class ReservationDto
{
    public int ReservationId { get; set; }
    public DateTime ReservationDate { get; set; } 
    public int CustomerId { get; set; } 
    public DateTime CheckInDate { get; set; }
    public int RoomId { get; set; }
    
    public DateTime CheckOutDate { get; set; }
    
    public decimal TotalPrice { get; set; }
    
}