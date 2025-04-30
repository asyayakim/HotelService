using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Db.Model;

public class Review
{
    [Key]
    public int ReviewId { get; set; }
    [ForeignKey("Hotel")]
    public int HotelId { get; set; }
    [ForeignKey("ReservationId")]
    public int ReservationId { get; set; }
    public Hotel Hotel { get; set; }
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    [Range(1, 10)]
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    
}