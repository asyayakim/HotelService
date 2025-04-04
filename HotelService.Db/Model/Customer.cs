using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelService.Db.Model;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }

    
}