using HotelService.Db.DTOs;
using HotelService.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;
[ApiController]
[Route("[controller]")]
//[Authorize(Roles = "User")]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public ReservationController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    
    //temporally, testing only
    [AllowAnonymous]
    [HttpPost("hotel/{id:int}")]
    public async Task<IActionResult> ReservationAsync(int id,[FromBody] ReservationDto request)
    {
        try
        {
            var reservation = await _reservationService.ReservationProcessAsync(request);
            if (reservation == null)
            {
                return BadRequest();
            }
            return Ok(reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [AllowAnonymous]
    [HttpDelete("reservation")]
    public async Task<IActionResult> ReservationCancellingAsync([FromBody] ReservationDto request)
    {
        try
        {
            var reservation = await _reservationService.CancelReservationAsync(request);
            if (reservation == null)
            {
                return BadRequest();
            }
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("reservations-by-user{roomId:int}")]
    public async Task<IActionResult> GetHotelById(int roomId)
    {
        try
        {
            var reservations = await _reservationService.GetAllReservationsByIdAsync(roomId);
            return Ok(reservations);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetHotels: {e.Message}\n{e.StackTrace}"); 
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }
   
    [HttpGet("reservations")]
    public async Task<IActionResult> GetAllReservations()
    {
        try
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetHotels: {e.Message}\n{e.StackTrace}"); 
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }
    [AllowAnonymous]
    [HttpGet("available-date/{id:int}")]
    public async Task<IActionResult> GetHotelByRoomId(int id)
    {
        try
        {
            var reservations = await _reservationService.GetAllReservationsByRoomIdAsync(id);
            return Ok(reservations);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in GetHotels: {e.Message}\n{e.StackTrace}"); 
            return StatusCode(500, $"An error occurred: {e.Message}");
        }
    }
}