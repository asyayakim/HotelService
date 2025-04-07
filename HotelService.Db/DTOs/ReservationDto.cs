using System.ComponentModel.DataAnnotations;

namespace HotelService.Db.DTOs;

public class ReservationDto
{
    public string HotelName { get; set; }
    [Required]
    public DateTime CheckInDate { get; set; }
    [Required]
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    
}