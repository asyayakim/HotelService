using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotelService.Db.Model;

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }

    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
    
    [ForeignKey("Room")]
    public int RoomId { get; set; }
    [JsonIgnore] 
    public Room Room { get; set; }
    
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    [JsonIgnore]
    public Customer Customer { get; set; }
    [Required]
    [Column(TypeName = "date")]
    public DateOnly CheckInDate { get; set; }
    [Required]
    [Column(TypeName = "date")]
    public DateOnly CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    [ForeignKey("PaymentMethod")] 
    public int? PaymentMethodId { get; set; }
    [JsonIgnore]
    public PaymentMethod PaymentMethod { get; set; }
    public int AdultsCount { get; set; }
}