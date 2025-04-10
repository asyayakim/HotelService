using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Logic;

public class HotelRepository
{
    private readonly AppDbContext _appDbContextdb;

    public HotelRepository(AppDbContext appDbContextdb)
    {
        _appDbContextdb = appDbContextdb;
    }

    public async Task<Hotel> AddHotelAsync(HotelCreateDto hotel)
    {
        var existingHotel = await _appDbContextdb.Hotels
            .FirstOrDefaultAsync(h => h.Name == hotel.Name);
    
        if (existingHotel != null)
        {
            return null;
        }

        var newHotel = new Hotel
        {
            Name = hotel.Name,
            Description = hotel.Description,
            ThumbnailUrl = hotel.ThumbnailUrl,
            Price = hotel.Price,
            IsActive = hotel.IsActive,
        };
        await _appDbContextdb.Hotels.AddAsync(newHotel);
        await _appDbContextdb.SaveChangesAsync();
        if (hotel.Rooms != null && hotel.Rooms.Any())
        {
            var newRooms = hotel.Rooms.Select(room => new Room
            {
                RoomType = room.RoomType,
                PricePerNight = room.PricePerNight,
                ThumbnailRoom = room.ThumbnailRoom,
                HotelId = newHotel.HotelId
            }).ToList();

            await _appDbContextdb.Rooms.AddRangeAsync(newRooms);
            await _appDbContextdb.SaveChangesAsync();
        }

        return newHotel;
    }

    public async Task<Room?> AddRoomsAsync(Room room)
    {
        var hotel =  await _appDbContextdb.Hotels.FirstOrDefaultAsync(h => h.HotelId == room.HotelId);
        if (hotel == null)
            return null;

        await _appDbContextdb.Rooms.AddAsync(room);
        await _appDbContextdb.SaveChangesAsync();

        return room;
    }

    public async Task<List<Hotel>> AddManyHotelsAsync(List<HotelCreateDto> hotels)
    {
        var addedHotels = new List<Hotel>();
        var addedRooms = new List<Room>();
        foreach (var hotel in hotels)
        {
            var exists = await _appDbContextdb.Hotels
                .AnyAsync(h => h.Name == hotel.Name);
            if (!exists)
            {
                var newHotel = new Hotel
                {
                    Name = hotel.Name,
                    Description = hotel.Description,
                    ThumbnailUrl = hotel.ThumbnailUrl,
                    Price = hotel.Price,
                    IsActive = hotel.IsActive
                };

                await _appDbContextdb.Hotels.AddAsync(newHotel);
                await _appDbContextdb.SaveChangesAsync();
                addedHotels.Add(newHotel);
                if (hotel.Rooms != null && hotel.Rooms.Any())
                {
                    newHotel.Rooms = hotel.Rooms.Select(r => new Room
                    {
                        RoomType = r.RoomType,
                        PricePerNight = r.PricePerNight,
                        ThumbnailRoom = r.ThumbnailRoom
                    }).ToList();
                    await _appDbContextdb.Rooms.AddRangeAsync(addedRooms);
                    await _appDbContextdb.SaveChangesAsync();
                }
            }
        }
        return addedHotels;
    }
}