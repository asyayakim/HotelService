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

    public async Task<HotelSendDto?> GetHotelsByIdAsync(int id)
    {
        var searchedHotel = await _dbContext.Hotels.Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.HotelId == id);
        if (searchedHotel == null)
            return null;
        var hotelDtos = new HotelSendDto
        {
            HotelId = searchedHotel.HotelId,
            Name = searchedHotel.Name,
            Description = searchedHotel.Description,
            ThumbnailUrl = searchedHotel.ThumbnailUrl,
            Price = searchedHotel.Price,
            Address = searchedHotel.Address,
            City = searchedHotel.City,
            Country = searchedHotel.Country,
            PostalCode = searchedHotel.PostalCode,
            IsActive = searchedHotel.IsActive,
            Rooms = searchedHotel.Rooms?.Select(r => new RoomSendDto
            {
                RoomId = r.RoomId,
                RoomType = r.RoomType,
                PricePerNight = r.PricePerNight,
                ThumbnailRoom = r.ThumbnailRoom
            }).ToList()
        };
        return hotelDtos;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<string?> FindCustomerAndReturnImage(User user)
    {
       var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.UserId == user.UserId);
       var image = customer.PictureUrl;
       return image;
    }
}