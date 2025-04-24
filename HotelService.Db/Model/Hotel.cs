using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HotelService.Db.DTOs;

namespace HotelService.Db.Model;

public class Hotel
{
    [Key]
    public int HotelId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public string ThumbnailUrl { get; set; } // add list of images
    public double Price { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public bool IsActive { get; set; } = true;
    [JsonIgnore]
    public ICollection<FavoriteHotels> FavoriteHotels { get; set; } = new List<FavoriteHotels>();
    [JsonIgnore]
    public ICollection<Room>? Rooms { get; set; } = new List<Room>();
    [JsonIgnore]
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}