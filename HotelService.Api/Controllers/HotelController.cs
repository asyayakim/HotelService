using System.Security.Claims;
using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using HotelService.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;


namespace HotelService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly HotelRepository _service;
    private readonly IDistributedCache _cache;
    private readonly AppDbContext _context;
    private readonly DbRepository _dbRepository;

    public HotelController(AppDbContext context, DbRepository dbRepository,
        HotelRepository service, IDistributedCache cache)
    {
        _context = context;
        _dbRepository = dbRepository;
        _service = service;
        _cache = cache;
    }

    [HttpGet("all-hotels")]
    public async Task<IActionResult> SearchHotels([FromQuery] HotelSearchDto searchDto)
    {
        try
        {
            var (hotels, totalCount) = await _service.GetHotelsPaginatedFromDbAsync(searchDto);

            var response = new
            {
                TotalRecords = totalCount,
                PageNumber = searchDto.PageNumber,
                PageSize = searchDto.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / searchDto.PageSize),
                Hotels = hotels
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetHotels: {e.Message}\n{e.StackTrace}");
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }


    [HttpGet("from-db/{id:int}")]
    public async Task<IActionResult> GetHotelByIdFromDb(int id)
    {
        string cacheKey = $"hotel_{id}";
        try
        {
            var cachedHotelJson = await _cache.GetStringAsync(cacheKey);
            if (cachedHotelJson != null)
            {
                var hotelDto = JsonSerializer.Deserialize<HotelSendDto>(cachedHotelJson);
                return Ok(hotelDto);
            }

            var hotel = await _service.GetHotelsByIdAsync(id);
            if (hotel == null)
                return NotFound($"Hotel with ID {id} not found.");
            var serialized = JsonSerializer.Serialize(hotel);
            await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // cache timeout
            });
            
            return Ok(hotel);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetHotels: {e.Message}\n{e.StackTrace}");
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }

    [HttpPost("create-many")]
    public async Task<IActionResult> CreateManyHotels([FromBody] List<HotelCreateDto> hotels)
    {
        try
        {
            var hotelsDb = await _service.AddManyHotelsAsync(hotels);
            if (hotelsDb == null)
                return BadRequest("Hotels with the same name already exists.");
            return Ok(hotelsDb);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateHotel([FromBody] HotelCreateDto hotel)
    {
        try
        {
            var hotelDb = await _service.AddHotelAsync(hotel);
            if (hotelDb == null)
                return BadRequest("Hotel with the same name already exists.");
            return Ok(hotelDb);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }

    [HttpPost("add-rooms")]
    public async Task<IActionResult> AddRoomsToHotelAsync([FromBody] Room room)
    {
        try
        {
            var hotelDb = await _service.AddRoomsAsync(room);
            if (hotelDb == null)
                return BadRequest("Hotel is not exist.");
            return Ok(hotelDb);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }

    //[Authorize(Roles = "HotelManager")]
    [HttpPatch("change-hotel-data")]
    public async Task<IActionResult> AddReservationAsync([FromBody] HotelSendDto dto)
    {
        try
        {
            //var userIdData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (string.IsNullOrEmpty(userIdData)) return Unauthorized();
            var hotelDataToEdit = await _service.ChangeHotelDataAsync(dto);
            return Ok(new
            {
                success = true,
                message = "Hotel data changed successfully.",
                updatedHotel = hotelDataToEdit
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}