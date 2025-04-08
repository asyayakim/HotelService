using HotelService.Db.Model;

namespace HotelService.Db.DTOs;

public class CustomerDto
{
    public string UserId { get; set; }
    public int ReservationId { get; set; }
}