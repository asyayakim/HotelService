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
        await _appDbContextdb.Hotels.AddAsync(hotel);
        await _appDbContextdb.SaveChangesAsync();
        return hotel;
    }
}