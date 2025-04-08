using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> ReservationProcessAsync(ReservationDto request)
    {
        var customer = await _appDbContext.Customers.FindAsync(request.UserId);
        var newReservation = new Reservation()
        {
            ReservationDate = DateTime.Today,
            CustomerId = request.UserId,
            Customer = customer,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            TotalPrice = request.TotalPrice,
            Status = "pending"
        };
        await _appDbContext.Reservations.AddAsync(newReservation);
        await _appDbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> CancelReservationAsync(CustomerDto request)
    {
        var reservation = await _appDbContext.Reservations.FindAsync(request.ReservationId);
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
    public async Task<Reservation?> GetAllReservationsAsync()
    {
        return await _appDbContext.Reservations.FirstOrDefaultAsync();
    }
}