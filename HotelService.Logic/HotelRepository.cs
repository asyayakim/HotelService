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
            Address = hotel.Address,
            City = hotel.City,
            Country = hotel.Country,
            PostalCode = hotel.PostalCode
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
        var hotel = await _appDbContextdb.Hotels.FirstOrDefaultAsync(h => h.HotelId == room.HotelId);
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
                    IsActive = hotel.IsActive,
                    Address = hotel.Address,
                    City = hotel.City,
                    Country = hotel.Country,
                    PostalCode = hotel.PostalCode
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

    public async Task<(List<HotelSendDto> hotels, int TotalCount)> GetHotelsPaginatedFromDbAsync(int pageNumber,
        int pageSize)
    {
        int skipCount = (pageNumber - 1) * pageSize;
        int totalCount = await _appDbContextdb.Hotels.CountAsync();
        var hotels = await _appDbContextdb.Hotels
            .Include(h => h.Rooms)
            .OrderBy(h => h.HotelId)
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();
        var hotelDtos = hotels.Select(h => new HotelSendDto
        {
            HotelId = h.HotelId,
            Name = h.Name,
            Description = h.Description,
            ThumbnailUrl = h.ThumbnailUrl,
            Price = h.Price,
            IsActive = h.IsActive,
            Address = h.Address,
            City = h.City,
            Country = h.Country,
            PostalCode = h.PostalCode,
            Rooms = h.Rooms?.Select(r => new RoomSendDto
            {
                RoomId = r.RoomId,
                RoomType = r.RoomType,
                PricePerNight = r.PricePerNight,
                ThumbnailRoom = r.ThumbnailRoom
            }).ToList()
        }).ToList();

        return (hotelDtos, totalCount);
    }
}