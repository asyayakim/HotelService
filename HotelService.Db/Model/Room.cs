using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Db.Model;

public class Room
{
    [Key]
    public int RoomId { get; set; }
    [ForeignKey("Hotel")]
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }
    [Required]
    public string RoomType { get; set; }
    public decimal PricePerNight { get; set; }
  
    public ICollection<Reservation> Reservations { get; set; }
}