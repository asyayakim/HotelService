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
    public int PhoneNumber { get; set; }
    public DateOnly? DateOfBirth { get; set; } 
    
    public ICollection<Reservation> Reservations { get; set; }
    public ICollection<PaymentMethod> PaymentMethods { get; set; }
    
}