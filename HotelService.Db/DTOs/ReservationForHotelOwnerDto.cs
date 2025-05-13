namespace HotelService.Db.DTOs;

public class ReservationForHotelOwnerDto
{
    public int? ReservationId { get; set; }
    public int CustomerId { get; set; } 
    public DateOnly CheckInDate { get; set; }
    public int RoomId { get; set; }
    
    public DateOnly CheckOutDate { get; set; }
    
    public decimal TotalPrice { get; set; }

    public int? AdultsCount { get; set; }
    public string? Status { get; set; }
   
}