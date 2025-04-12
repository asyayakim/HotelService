using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HotelService.Logic;

public class ReservationService
{
    private readonly DbRepository _dbRepository;
    private readonly AppDbContext _appDbContext;

    public ReservationService(DbRepository dbRepository, AppDbContext appDbContext)
    {
        _dbRepository = dbRepository;
        _appDbContext = appDbContext;
    }

    public async Task<Reservation> ReservationProcessAsync(ReservationDto request)
    {
        var customer = await _appDbContext.Customers.FirstOrDefaultAsync(r => 
            r.CustomerId == request.CustomerId);
        
        var newReservation = new Reservation()
        {
            ReservationDate = DateTime.Today,
            CustomerId = request.CustomerId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            TotalPrice = request.TotalPrice,
            Status = "pending"
        };
        await _appDbContext.Reservations.AddAsync(newReservation);
        await _appDbContext.SaveChangesAsync();
        return newReservation;
    }
    
    public async Task<bool> CancelReservationAsync(ReservationDto request)
    {
        var reservation = await _appDbContext.Reservations.FirstOrDefaultAsync(h => h.CustomerId 
            == request.CustomerId && 
            h.ReservationId == request.ReservationId);
        if (reservation == null)
        {
            return false;
        }
        reservation.Status = "cancelled";
        await _appDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Reservation>> GetAllReservationsByIdAsync(int id)
    {
        var reservationsByUser = _appDbContext.Reservations.Where(r => r.CustomerId == id);
        return await reservationsByUser.ToListAsync();
    }
    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await _appDbContext.Reservations.ToListAsync();
    }
}