using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelService.Db.Model;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public string PictureUrl { get; set; } = string.Empty;
  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int LoyaltyPoints { get; set; } = 10;
    public string PhoneNumber { get; set; }
    [Column(TypeName = "date")]
    public DateOnly? DateOfBirth { get; set; } 
    [JsonIgnore]
    public ICollection<Reservation> Reservations { get; set; }
    [JsonIgnore]
    public ICollection<PaymentMethod> PaymentMethods { get; set; }
    [JsonIgnore]
    public ICollection<Review> Reviews { get; set; }
    [JsonIgnore]
    public ICollection<FavoriteHotels> FavoriteHotels { get; set; } = new List<FavoriteHotels>();
}