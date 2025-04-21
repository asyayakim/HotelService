using System.Security.Claims;
using HotelService.Db.DTOs;
using HotelService.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class FavoriteController : ControllerBase
{
    private readonly CustomerService _customerService;

    public FavoriteController(CustomerService customerService)
    {
        _customerService = customerService;
    }
    [HttpPost]
    public async Task<IActionResult> AddToFavorites([FromBody] FavoriteDto favorite)
    {
        try
        {
            var userIdData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdData)) return Unauthorized();
           var result = await _customerService.AddToFavoritesAsync(favorite);
             if (result == null)
                 return Conflict("This hotel is already in favorites");
           return Ok("Hotel added to favorites");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPatch]
    public async Task<IActionResult> RemoveFromFavorites([FromBody] FavoriteDto favorite)
    {
        try
        {
            var userIdData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdData)) return Unauthorized();
            
            await _customerService.RemoveFromFavoritesAsync(favorite);
            return Ok("Hotel removed from favorites");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("all-by-user")]
    public async Task<IActionResult> GetFavoritesByUser()
    {
        try
        {
            var userIdData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdData)) return Unauthorized();
            var user = int.Parse(userIdData);
            var favorites = await _customerService.GetAllFavoritesByUser(user);
            return Ok(favorites);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllFavorites()
    {
        try
        {
            var favorites = await _customerService.GetAllFavoritesData();
            if (favorites == null)
                return NoContent();
            return Ok(favorites);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}