using HotelService.Db.DTOs;
using HotelService.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _reviewService;

    public ReviewController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }


    [HttpPost]
    public async Task<IActionResult> ReservationAsync([FromBody] ReviewDto request, ReservationDto reservationDto)
    {
        try
        {
            var review = await _reviewService.PostReviewAsync(request, reservationDto);
            if (review == null)
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
}