using System.Security.Claims;
using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using HotelService.Logic;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly HotelRepository _service;
    private readonly AppDbContext _context;
    private readonly CsvReaderService _csvReaderService;
    private readonly DbRepository _dbRepository;

    public HotelController(AppDbContext context, CsvReaderService csvReaderService, DbRepository dbRepository, HotelRepository service)
    {
        _context = context;
        _csvReaderService = csvReaderService;
        _dbRepository = dbRepository;
        _service = service;
    }

    // [HttpGet]
    // public async Task<IActionResult> GetHotels([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 4)
    // {
    //     try
    //     {
    //         var (hotels, totalCount) = await _csvReaderService.GetHotelsPaginatedAsync(pageNumber, pageSize);
    //
    //         var response = new
    //         {
    //             TotalRecords = totalCount,
    //             PageNumber = pageNumber,
    //             PageSize = pageSize,
    //             TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
    //             Hotels = hotels
    //         };
    //         return Ok(response);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine($"Error in GetHotels: {e.Message}\n{e.StackTrace}");
    //         return StatusCode(500, $"An error occurred: {e.Message}");
    //     }
    // }
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

    

    //  from csv
    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetHotelById(int id)
    // {
    //     try
    //     {
    //         var hotel = await _csvReaderService.GetHotelsByIdAsync(id);
    //         return Ok(hotel);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine($"Error in GetHotels: {e.Message}\n{e.StackTrace}");
    //         return StatusCode(500, $"An error occurred: {e.Message}");
    //     }
    // }

    [HttpGet("from-db/{id:int}")]
    public async Task<IActionResult> GetHotelByIdFromDb(int id)
    {
        try
        {
            var hotel = await _dbRepository.GetHotelsByIdAsync(id);
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
            return Ok(hotelDataToEdit);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        } 
    }
}