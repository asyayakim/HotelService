using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Db.Model;

public class Hotel
{
    [Key]
    public int HotelId { get; set; }
    
    [ForeignKey("Host")]
    public int HostId { get; set; }
    public Host Host { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    
    public string ThumbnailUrl { get; set; }
    public double LogPrice { get; set; }
    public bool HasThumbnail { get; set; } 
    
    public bool IsActive { get; set; } = true;
    public ICollection<Reservation> Reservations { get; set; }
}