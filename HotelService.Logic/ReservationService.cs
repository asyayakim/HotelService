using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Logic;

public class ReservationService
{
   
    private readonly AppDbContext _appDbContext;

    public ReservationService( AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Reservation> ReservationProcessAsync(ReservationDto request)
    {
        var newReservation = new Reservation()
        {
            CustomerId = request.CustomerId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            TotalPrice = request.TotalPrice,
            Status = "paid",
            AdultsCount = request.AdultsCount,
            RoomId = request.RoomId,
            PaymentMethodId = request.PaymentMethodId
        };
        var customer = await _appDbContext.Customers.FindAsync(request.CustomerId)
                       ?? throw new InvalidOperationException("Customer not found");
        if (request.PointsUsed > customer.LoyaltyPoints)
            throw new InvalidOperationException("Not enough loyalty points.");

        customer.LoyaltyPoints -= request.PointsUsed;

        await _appDbContext.Reservations.AddAsync(newReservation);
        await _appDbContext.SaveChangesAsync();
        return newReservation;
    }
    
    public async Task<bool> CancelReservationAsync(int userId, int reservationId)
    {
        var reservation = await _appDbContext.Reservations.FirstOrDefaultAsync(h => h.CustomerId 
            == userId && 
            h.ReservationId == reservationId);
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
    public async Task <List<ReservationDto>> GetAllReservationsByRoomIdAsync(int id)
    {
        var reservationToReturn = new List<ReservationDto>();
        var reservationsByRoom = await _appDbContext.Reservations.Where(r
            => r.RoomId == id).ToListAsync();
        var activeReservations = reservationsByRoom.Where(r => r.Status == "active" || r.Status == "paid");
        foreach (var reservation in activeReservations)
        {
            var reservationDate = new ReservationDto
            {
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
            };
            reservationToReturn.Add(reservationDate);
        }
        return reservationToReturn;
    }

    public async Task<List<Reservation>> GetAllReservationsByUserAsync( int userId)
    {
       var findCustomer = await _appDbContext.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
      var reservations = await _appDbContext.Reservations.Include(r=> r.Room).ThenInclude(h => h.Hotel).Where(r
          => r.CustomerId == findCustomer!.CustomerId).ToListAsync();
      
      return reservations;
    }

    public async Task<List<ReservationForHotelOwnerDto?>> GetAllActiveReservations(int hotelId)
    {
        var reservationToReturn = new List<ReservationForHotelOwnerDto>();
        var findHotel = await _appDbContext.Reservations.
            Include(r => r.Room).ToListAsync();
        var reservations = findHotel.Where(h
            => h.Room.HotelId == hotelId).ToList().Where(s => s.Status == "active" || s.Status == "paid");
        foreach (var reservation in reservations)
        {
            var reservationObject = new ReservationForHotelOwnerDto
            {
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                TotalPrice = reservation.TotalPrice,
                CustomerId = reservation.CustomerId,
                AdultsCount = reservation.AdultsCount,
                ReservationId = reservation.ReservationId,
                Status = reservation.Status,
                RoomId = reservation.Room.RoomId
            };
                reservationToReturn.Add(reservationObject);
        }

        return reservationToReturn!;
    }
}