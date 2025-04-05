using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Db.Model;

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }
    public DateTime ReservationDate { get; set; }
    
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    [Required]
    public DateTime CheckInDate { get; set; }
    [Required]
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }

}