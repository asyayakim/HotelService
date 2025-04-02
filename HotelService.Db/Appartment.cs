using System.ComponentModel.DataAnnotations;

namespace HotelService.Db;

public class Appartment
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    
    
}