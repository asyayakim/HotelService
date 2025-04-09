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
}