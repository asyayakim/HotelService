using HotelService.Db.Model;

namespace HotelService.Db.DTOs;

public class CustomerDto
{
    public string UserId { get; set; }
    public int ReservationId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
}