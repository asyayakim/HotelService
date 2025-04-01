using HotelService.Db;
using HotelService.Logic;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly CsvReaderService _csvReaderService;

    public HotelController(AppDbContext context, CsvReaderService csvReaderService)
    {
        _context = context;
        _csvReaderService = csvReaderService;
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
            Console.WriteLine($"Error in GetHotels: {e.Message}\n{e.StackTrace}"); // Log error to console
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }

}