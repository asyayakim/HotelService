using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Logic;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly CsvReaderService _csvReaderService;
    private readonly DbRepository _dbRepository;

    public HotelController(AppDbContext context, CsvReaderService csvReaderService, DbRepository dbRepository)
    {
        _context = context;
        _csvReaderService = csvReaderService;
        _dbRepository = dbRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetHotels([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 4)
    {
        try
        {
            var (hotels, totalCount) = await _csvReaderService.GetHotelsPaginatedAsync(pageNumber, pageSize);

            var response = new
            {
                TotalRecords = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHotelById(int id)
    {
        try
        {
            var hotel = await _csvReaderService.GetHotelsByIdAsync(id);
            return Ok(hotel);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetHotels: {e.Message}\n{e.StackTrace}"); 
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }
    [HttpGet("from-db{id:int}")]
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
}