using HotelService.Db;
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

    public async Task<Hotel?> AddHotelAsync(Hotel hotel)
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
            foreach (var room in hotel.Rooms)
            {
                newHotel.Rooms.Add(new Room
                {
                    RoomType = room.RoomType,
                    PricePerNight = room.PricePerNight,
                    ThumbnailRoom = room.ThumbnailRoom,
                    HotelId = newHotel.HotelId
                });
            }

            await _appDbContextdb.SaveChangesAsync();
        }

        return newHotel;
    }

    public async Task<Room?> AddRoomsAsync(Room room)
    {
        var hotel =  await _appDbContextdb.Hotels.FirstOrDefaultAsync(h => h.HotelId == room.HotelId);
        if (hotel == null)
            return null;
        if (hotel == null)
            return null;

        await _appDbContextdb.Rooms.AddAsync(room);
        await _appDbContextdb.SaveChangesAsync();

        return room;
    }

    public async Task<List<Hotel>> AddManyHotelsAsync(List<Hotel> hotels)
    {
        var addedHotels = new List<Hotel>();
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
                addedHotels.Add(newHotel);
            }
        }
        if (addedHotels.Any())
            await _appDbContextdb.SaveChangesAsync();

        return addedHotels;
    }
}