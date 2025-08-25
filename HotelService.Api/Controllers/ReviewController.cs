using System.Security.Claims;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
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
    public async Task<IActionResult> ReservationAsync([FromBody] ReviewDto request)
    {
        try
        {
            var review = await _reviewService.PostReviewAsync(request);
            if (review == false)
            {
                return BadRequest("Reservation not found, " +
                                  "customer not found, or reservation not completed.");
            }

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("hotel/{hotelId:int}")]
    public async Task<IActionResult> GetReviewsByHotelAsync(int hotelId)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByHotelAsync(hotelId);
            if (reviews.Count == 0)
            {
                return NotFound($"No reviews found for hotel with ID {hotelId}.");
            }

            return Ok(reviews);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetReviewsByUserAsync(int userId)
    {
        try
        {
            var userIdData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdData)) return Unauthorized();
            var reviews = await _reviewService.GetReviewsByUserAsync(userId);
            if (reviews.Count == null || !reviews.Any())
            {
                return Ok(new List<ReviewDto>());
            }

            return Ok(reviews);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("edit/{reviewId:int}")]
    public async Task<ActionResult<CustomerDto>> ChangeData([FromBody] ReviewDto reviewDto)
    {
        try
        {
            var userIdData = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdData)) return Unauthorized();

            var updatedReview = await _reviewService.ChangeDataAsync(reviewDto);
            if (updatedReview == null)
                return BadRequest();
            return Ok(updatedReview);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [AllowAnonymous]
    [HttpDelete]
    public async Task<IActionResult> DeleteReviewAsync( [FromQuery] int reviewId)
    {
        try
        {
            var review = await _reviewService.CancelReviewAsync( reviewId);
            
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}