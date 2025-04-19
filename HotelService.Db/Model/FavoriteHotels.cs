using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelService.Db.Model;

public class FavoriteHotels
{
    [ForeignKey("Hotel")]
    public int HotelId { get; set; } 
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    [JsonIgnore]
    public Customer Customer { get; set; }

    [JsonIgnore]
    public Hotel Hotel { get; set; }

    public bool IsFavorite { get; set; } = true;

    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public DateTime? DateRemoved { get; set; } = null;
}