using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HotelService.Logic;

public class CustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> ChangeData(CustomerDto customerDto)
    {
        var customerToChange =
            await _context.Customers.FirstOrDefaultAsync(c =>
                c.UserId == customerDto.UserId);
        var updatedCustomer = new Customer
        {
            FirstName = customerToChange.FirstName,
            LastName = customerToChange.LastName,
            PhoneNumber = customerToChange.PhoneNumber,
            DateOfBirth = customerToChange.DateOfBirth,
            UserId = customerDto.UserId,
        };
        await _context.Customers.AddAsync(updatedCustomer);
        await _context.SaveChangesAsync();
        return updatedCustomer;
    }

    public async Task<Customer> AddNewCustomerAsync(CustomerDto customerDto)
    {
        var newCustomer = new Customer
        {
            FirstName = customerDto.FirstName,
            LastName = customerDto.LastName,
            PhoneNumber = customerDto.PhoneNumber,
            DateOfBirth = customerDto.DateOfBirth?.ToDateTime(TimeOnly.MinValue),
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
            return null;
        await _context.FavoriteHotels.AddAsync(newFavorite);
        await _context.SaveChangesAsync();
        return newFavorite;
    }

    public async Task RemoveFromFavoritesAsync(FavoriteDto favorite)
    {
        var findCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == favorite.UserId);
        if (findCustomer == null)
            return;
        var existingFav = _context.FavoriteHotels.FirstOrDefault(h =>
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
}