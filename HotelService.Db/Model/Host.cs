using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Db.Model;

public class Host
{
    [Key]
    public int HostId { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    public string CompanyName { get; set; }
    
    public ICollection<Hotel> Hotels { get; set; }
}