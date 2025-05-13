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

    public async Task<(List<HotelSendDto> hotels, int TotalCount)> GetHotelsPaginatedFromDbAsync(HotelSearchDto dto)
    {
        var query = _appDbContextdb.Hotels.Include(h => h.Rooms).AsQueryable();
        if (!string.IsNullOrEmpty(dto.SearchText))
        {
            var search = dto.SearchText.ToLower();
            query = query.Where(h =>
                h.Name.ToLower().Contains(search) ||
                h.City.ToLower().Contains(search));
        }
        if (dto.Price.HasValue)
            query = query.Where(h => h.Price == dto.Price);
        int totalCount = await query.CountAsync();
        int skip = (dto.PageNumber - 1) * dto.PageSize;
        var hotels = await query
            .OrderBy(h => h.HotelId)
            .Skip(skip)
            .Take(dto.PageSize)
            .ToListAsync();

        var hotelDtos = HotelSendDtos(hotels);
        return (hotelDtos, totalCount);
    }
    
    
    private static List<HotelSendDto> HotelSendDtos(List<Hotel> hotels)
    {
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
        return hotelDtos;
    }

    public async Task<HotelSendDto?> ChangeHotelDataAsync(HotelSendDto dto)
    {
        var hotels = await _appDbContextdb.Hotels
                .Include(r => r.Rooms).ToListAsync();
        var correctHotel = hotels.FirstOrDefault(h => h.HotelId == dto.HotelId);
        if (correctHotel == null)
        {
            return null;
        }
        if (!string.IsNullOrEmpty(dto.Address))
            correctHotel.Address = dto.Address;
        if (!string.IsNullOrEmpty(dto.City))
            correctHotel.City = dto.City;
        if (!string.IsNullOrEmpty(dto.Country))
            correctHotel.Country = dto.Country;
        if (!string.IsNullOrEmpty(dto.PostalCode))
            correctHotel.PostalCode = dto.PostalCode;
        if (!string.IsNullOrEmpty(dto.Description))
            correctHotel.Description = dto.Description;
        if (dto.Price.HasValue)
            correctHotel.Price = dto.Price.Value;
        // if (dto.Rooms != null && dto.Rooms.Any())
        //     foreach (var room in dto.Rooms)
        //         
        await _appDbContextdb.SaveChangesAsync();
        return new HotelSendDto
        {
            HotelId = correctHotel.HotelId,
            Address = correctHotel.Address,
            City = correctHotel.City,
            Country = correctHotel.Country,
            PostalCode = correctHotel.PostalCode,
            Description = correctHotel.Description,
            Price = correctHotel.Price
        };
    }
    public async Task<HotelSendDto?> GetHotelsByIdAsync(int id)
    {
        var searchedHotel = await _appDbContextdb.Hotels.Include(h => h.Rooms)
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
}