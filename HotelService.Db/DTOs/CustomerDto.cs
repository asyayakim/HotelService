using System.ComponentModel.DataAnnotations;
using HotelService.Db.Model;

namespace HotelService.Db.DTOs;

public class CustomerDto
{
    [Required]
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Phone]
    public int PhoneNumber { get; set; }
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }
    
    
}