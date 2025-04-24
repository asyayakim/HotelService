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

    public async Task<object?> PostReviewAsync(ReviewDto request, ReservationDto reservationDto)
    {
        var customerId = _context.Customers.FirstOrDefaultAsync(
            c => c.UserId == request.UserId);
        var targetedReservation = _context.Reservations.FirstOrDefaultAsync(
        r=>r.ReservationId == reservationDto.ReservationId);
        if (targetedReservation.Status = "completed") 
            Reservation reservation = reservationDto;
    }
}