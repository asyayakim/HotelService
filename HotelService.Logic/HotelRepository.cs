using HotelService.Db;
using HotelService.Db.Model;

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
        var name = _appDbContextdb.Hotels.FindAsync(hotel.Name);
        if (name != null)
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
        return hotel;
    }
}