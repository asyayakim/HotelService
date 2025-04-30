using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Logic;

public class ReviewService
{
    private readonly AppDbContext _context;

    public ReviewService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Review> PostReviewAsync(ReviewDto request)
    {
        var customerId = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == request.UserId);
        var targetReservation = await _context.Reservations.
            Include(r => r.Room)
            .ThenInclude(room => room.Hotel)
            .FirstOrDefaultAsync(r => r.ReservationId == request.ReservationId);
        if (targetReservation == null)
            throw new Exception("Reservation not found");
        if (customerId == null)
            throw new Exception("Customer not found");
        if (targetReservation.Status != "completed")
            throw new Exception("Status not completed");
        var existingReview =
            await _context.Reviews.FirstOrDefaultAsync(r => r.ReservationId == request.ReservationId);
        if (existingReview != null)
            throw new Exception("Review already exists");

        var review = new Review
        {
            CustomerId = customerId.CustomerId,
            HotelId = targetReservation.Room.HotelId,
            ReservationId = targetReservation.ReservationId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt  = DateTime.UtcNow,
        };
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<List<Review>> GetReviewsByHotelAsync(int hotelId)
    {
        var hotelReviews = await _context.Reviews.Where(h => h.HotelId == hotelId)
            .Include(c => c.Customer)
            .ToListAsync();
        return hotelReviews;
    }

    public async Task<List<Review?>> GetReviewsByUserAsync(int userId)
    {
        var customerToFind = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        
        var hotelReviews = await _context.Reviews.Where(c
            => customerToFind != null && c.CustomerId == customerToFind.CustomerId).ToListAsync();
        return hotelReviews!;
    }

    public async Task<Review?> ChangeDataAsync(ReviewDto reviewDto)
    {
        var reviewToChange = _context.Reviews.FirstOrDefault
            (r => r.ReservationId == reviewDto.ReservationId);
        if (!string.IsNullOrEmpty(reviewToChange.Comment))
            reviewToChange.Comment = reviewDto.Comment;
        await _context.SaveChangesAsync();
        return reviewToChange;
    }
}