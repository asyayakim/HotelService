using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Logic;

public class ReviewService
{
    private readonly AppDbContext _context;

    public ReviewService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Review> PostReviewAsync(ReviewDto request, ReservationDto reservationDto)
    {
        var customerId = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == request.UserId);
        var targetReservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.ReservationId == reservationDto.ReservationId);
        if (targetReservation == null)
            throw new Exception("Reservation not found");
        if (customerId == null)
            throw new Exception("Customer not found");
        if (targetReservation.Status != "completed")
            throw new Exception("Status not completed");
        var existingReview =
            await _context.Reviews.FirstOrDefaultAsync(r => r.ReservationId == reservationDto.ReservationId);
        if (existingReview != null)
            throw new Exception("Review already exists");

        var review = new Review
        {
            CustomerId = customerId.CustomerId,
            HotelId = request.HotelId,
            ReservationId = targetReservation.ReservationId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt = new DateOnly()
        };
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }
}