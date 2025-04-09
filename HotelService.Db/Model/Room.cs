using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelService.Db.Model;

public class Room : IEnumerable
{
    [Key]
    public int RoomId { get; set; }
    [ForeignKey("Hotel")]
    public int HotelId { get; set; }
   
    [JsonIgnore]
    public Hotel Hotel { get; set; }
    [Required]
    public string RoomType { get; set; }
    public decimal PricePerNight { get; set; }

    public string ThumbnailRoom { get; set; }
    [JsonIgnore]
    public ICollection<Reservation> Reservations { get; set; }

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}