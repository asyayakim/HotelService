namespace HotelService.Db.DTOs;

public class ReviewDto
{
    public int HotelId { get; set; }

    public int UserId { get; set; }

    public int Rating { get; set; }
    public string Comment { get; set; }
}