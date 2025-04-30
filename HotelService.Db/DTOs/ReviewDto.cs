namespace HotelService.Db.DTOs;

public class ReviewDto
{
    public int? ReservationId { get; set; }

    public int? UserId { get; set; }

    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool? IsActive { get; set; }
}