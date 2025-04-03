using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Db.Model;

public class Host
{
    [Key]
    public int HostId { get; set; }
    [ForeignKey("Account")]
    public int UserId { get; set; }
    public Account Account { get; set; }
    public string CompanyName { get; set; }
    
    public ICollection<Hotel> Hotels { get; set; }
}