using System.ComponentModel.DataAnnotations;

namespace HotelService.Db;

public class Customer
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
 
    
}