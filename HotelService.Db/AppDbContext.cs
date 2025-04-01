using Microsoft.EntityFrameworkCore;

namespace HotelService.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions
        <AppDbContext> options)
        : base(options)
    {
    }
}