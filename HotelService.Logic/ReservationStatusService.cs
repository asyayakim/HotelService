using HotelService.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HotelService.Logic;

public class ReservationStatusService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<ReservationStatusService> _logger;

    public ReservationStatusService(
        IServiceProvider services,
        ILogger<ReservationStatusService> logger)
    {
        _services = services;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var reservations = await dbContext.Reservations
                .Where(r => r.CheckOutDate < DateOnly.FromDateTime(DateTime.UtcNow)
                            && (r.Status == "confirmed" || r.Status == "paid")).Include(r => r.Customer)
                .ToListAsync(stoppingToken);
            foreach (var reservation in reservations)
            {
                try
                {
                    var totalNight = reservation.CheckOutDate.DayNumber - reservation.CheckInDate.DayNumber;
                    var points = totalNight * 10;
                    reservation.Customer.LoyaltyPoints += points;
                    reservation.Status = "completed";
                    _logger.LogInformation(
                        "Added {points} points to customer {customerId} for reservation {reservationId}",
                        points, reservation.CustomerId, reservation.ReservationId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating reservations");
                }

                var updatedCount = await dbContext.SaveChangesAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }
    }
}