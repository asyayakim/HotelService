using System.ComponentModel.DataAnnotations;
using HotelService.Db.Model;

namespace HotelService.Db.DTOs;

public class CustomerDto
{
    [Required]
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PhoneNumber { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    
    
}