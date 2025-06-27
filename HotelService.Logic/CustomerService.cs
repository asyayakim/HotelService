using System.Text.Json;
using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace HotelService.Logic;

public class CustomerService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly string? _uploadUrl;

    public CustomerService(AppDbContext context, HttpClient httpClient, IConfiguration config)
    {
        _context = context;
        _httpClient = httpClient;
        _uploadUrl = config["ExternalApi:CloudinaryUploadUrl"];
    }

    public async Task<IActionResult?> ChangeData(CustomerDto customerDto)
    {
        var customerToChange =
            await _context.Customers.FirstOrDefaultAsync(c =>
                c.UserId == customerDto.UserId);
        
        var userToChange = await _context.Users.FirstOrDefaultAsync(c
            =>c.UserId == customerDto.UserId);
        if (!string.IsNullOrEmpty(customerDto.FirstName))
                customerToChange.FirstName = customerDto.FirstName;
        if (!string.IsNullOrEmpty(customerDto.LastName))
            customerToChange.LastName = customerDto.LastName;
        if(!string.IsNullOrEmpty(customerDto.PhoneNumber))
            customerToChange.PhoneNumber = customerDto.PhoneNumber;
        if(!string.IsNullOrEmpty(customerDto.Email))
            userToChange.Email = customerDto.Email;
        if (customerDto.DateOfBirth.HasValue)
        {
            customerToChange.DateOfBirth = customerDto.DateOfBirth.Value;
        }
        if(!string.IsNullOrEmpty(userToChange.Username))
            userToChange.Username = userToChange.Username;
        if(!string.IsNullOrEmpty(customerDto.Password))
        {
            string hashedPassword;
            hashedPassword = BCrypt.Net.BCrypt.HashPassword( customerDto.Password);
            userToChange.PasswordHash = hashedPassword;
        }
        await _context.SaveChangesAsync();
        return null;
    }

    public async Task<Customer> AddNewCustomerAsync(CustomerDto customerDto)
    {
        var phoneHashed =  BCrypt.Net.BCrypt.HashPassword( customerDto.PhoneNumber);
        var newCustomer = new Customer
        {
            FirstName = customerDto.FirstName,
            LastName = customerDto.LastName,
            PhoneNumber = phoneHashed,
            DateOfBirth = customerDto.DateOfBirth,
            UserId = customerDto.UserId
        };
        await _context.Customers.AddAsync(newCustomer);
        await _context.SaveChangesAsync();
        return newCustomer;
    }

    public async Task<FavoriteHotels> AddToFavoritesAsync(FavoriteDto favorite)
    {
        var findCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == favorite.UserId);

        var newFavorite = new FavoriteHotels
        {
            HotelId = favorite.HotelId,
            CustomerId = findCustomer.CustomerId,
            DateAdded = DateTime.UtcNow,
            IsFavorite = true,
        };
        var existingFavorite = await _context.FavoriteHotels.FirstOrDefaultAsync(h => h.HotelId == newFavorite.HotelId);
        if (existingFavorite != null)
        {
            existingFavorite.IsFavorite = true;
            existingFavorite.DateAdded = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existingFavorite;
        }
        
        await _context.FavoriteHotels.AddAsync(newFavorite);
        await _context.SaveChangesAsync();
        return newFavorite;
    }

    public async Task RemoveFromFavoritesAsync(FavoriteDto favorite)
    {
        var findCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == favorite.UserId);
        if (findCustomer == null)
            return;
        var existingFav = await _context.FavoriteHotels.FirstOrDefaultAsync(h =>
            h.HotelId == favorite.HotelId && h.CustomerId == findCustomer.CustomerId);
        if (existingFav == null)
            return;
        existingFav.IsFavorite = false;
        existingFav.DateRemoved = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<List<FavoriteHotels>> GetAllFavoritesByUser(int userIdData)
    {
        var customerToFind = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userIdData);
        if (customerToFind == null)
            return new List<FavoriteHotels>();
        var usersFav = await _context.FavoriteHotels.Where(h => h.CustomerId
                                                                == customerToFind.CustomerId)
            .ToListAsync();
        var userActualFav = await _context.FavoriteHotels.Where(
            f => f.IsFavorite == true).ToListAsync();
        return userActualFav;
    }

    public async Task<List<FavoriteHotels>> GetAllFavoritesData()
    {
        return await _context.FavoriteHotels.ToListAsync();
    }

    public async Task<string?> UploadImageAsync(IFormFile image, string publicId, int userId )
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(image.OpenReadStream()), "image", image.FileName);
        content.Add(new StringContent(publicId), "publicId");

        var response = await _httpClient.PostAsync(_uploadUrl, content);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorMsg = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Cloudinary error: " + errorMsg);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var imageUrl = doc.RootElement.GetProperty("url").GetString();
        var user = await _context.Customers.FirstOrDefaultAsync(c
            => c.UserId == userId);
        if (user != null)
        {
            user.PictureUrl = imageUrl;
            await _context.SaveChangesAsync();
        }
        return imageUrl;
    }

    public async Task<CustomerDto?> GetUserData(int userId)
    {
        var user = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        var userToSend = new CustomerDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            LoyalityPoints = user.LoyaltyPoints,
        };
        return userToSend;
    }
}