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

            var reservationsToUpdate = await dbContext.Reservations
                .Where(r => r.CheckOutDate < DateOnly.FromDateTime(DateTime.UtcNow) 
                            && (r.Status == "confirmed" || r.Status == "paid"))
                .ExecuteUpdateAsync(r => 
                        r.SetProperty(p => p.Status, "completed"), 
                    stoppingToken);

            _logger.LogInformation("Updated {count} reservations to completed", reservationsToUpdate);

            await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
        }
    }
}