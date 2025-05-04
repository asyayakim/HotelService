namespace HotelService.Db.DTOs;

public class UserDto
{
    public int UserId { get; set; }

    public string? Username { get; set; }
    
    public string? Email { get; set; }

    public string Role { get; set; } 
    public DateTime? RegistrationDate { get; set; }
    public string? ImageUrl { get; set; }
}