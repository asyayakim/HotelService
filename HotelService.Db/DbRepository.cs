using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Db;

public class DbRepository
{
    private readonly AppDbContext _dbContext;

    public DbRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUsersDataByEmailAsync(string requestEmail)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == requestEmail);
    }

    public async Task<User> AddNewUserDataAsync(string requestUsername, string hashedPassword, string requestEmail, string role)
    {
        var newUser =
            new User
            {
                Username = requestUsername,
                PasswordHash = hashedPassword,
                Email = requestEmail,
                Role = role,
                RegistrationDate = DateTime.UtcNow
            };
        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
        return newUser;
    }

    public async Task<User?> GetUsersDataAsync(string? requestUsername, string? requestEmail)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == requestUsername || u.Email == requestEmail);
    }

    public async Task<Hotel?> GetHotelsByIdAsync(int id)
    {
        var searchedHotel = await _dbContext.Hotels.FindAsync(id);
        if (searchedHotel == null)
            return null;
        var hotel = new Hotel()
        {
            HotelId = searchedHotel.HotelId,
            Name = searchedHotel.Name,
            Description = searchedHotel.Description,
            ThumbnailUrl = searchedHotel.ThumbnailUrl,
            Price = searchedHotel.Price,
            Reservations = searchedHotel.Reservations
        };
        return hotel;
    }
}