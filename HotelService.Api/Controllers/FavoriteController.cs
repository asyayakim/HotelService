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
            await _customerService.AddToFavoritesAsync(favorite);
             return Ok("Hotel added to favorites");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}