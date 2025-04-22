using System.ComponentModel.DataAnnotations;

namespace HotelService.Db.Model;

public class User
{
    [Key]
    public int UserId { get; set; }

    public string? Username { get; set; }
   
    [EmailAddress]
    public string Email { get; set; }

    public string? Role { get; set; } //Customer, Host, Admin

    public string PasswordHash { get; set; }
    public DateTime RegistrationDate { get; set; }
}