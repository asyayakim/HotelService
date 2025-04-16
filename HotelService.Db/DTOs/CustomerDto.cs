using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelService.Db.Model;

namespace HotelService.Db.DTOs;

public class CustomerDto
{
    [Required]
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string PhoneNumber { get; set; }
    [Column(TypeName = "date")]
    public DateOnly? DateOfBirth { get; set; }
    
    
}