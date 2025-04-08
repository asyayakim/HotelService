using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public string ThumbnailRoom { get; set; }
  
    public ICollection<Reservation> Reservations { get; set; }
}