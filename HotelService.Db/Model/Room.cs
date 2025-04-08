using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Db.Model;

public class Room
{
    [ForeignKey("Hotel")]
    public int HotelId { get; set; }
  
    
}