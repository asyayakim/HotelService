using System.ComponentModel.DataAnnotations;
using HotelService.Db.Model;

namespace HotelService.Db.DTOs;

public class ReservationDto
{
    public int? ReservationId { get; set; }
    public int CustomerId { get; set; } 
    public DateOnly CheckInDate { get; set; }
    public int RoomId { get; set; }
    
    public DateOnly CheckOutDate { get; set; }
    
    public decimal TotalPrice { get; set; }
    public int PaymentMethodId { get; set; }
    
}