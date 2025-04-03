using System.ComponentModel.DataAnnotations;

namespace HotelService.Db.Model;

public class Account
{
    [Key]
    public int AccountId { get; set; }
    [Required]
    public string? Username { get; set; }
   
    [EmailAddress]
    public string Email { get; set; }

    public string? Role { get; set; } //Customer, Host, Admin
    [Required]
    public string PasswordHash { get; set; }
    public DateTime RegistrationDate { get; set; }
}