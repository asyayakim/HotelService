using HotelService.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions
        <AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<FavoriteHotels> FavoriteHotels { get; set; }
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FavoriteHotels>()
            .HasKey(f => new { f.CustomerId, f.HotelId });

        modelBuilder.Entity<FavoriteHotels>()
            .HasOne(f => f.Customer)
            .WithMany(c => c.FavoriteHotels)
            .HasForeignKey(f => f.CustomerId);

        modelBuilder.Entity<FavoriteHotels>()
            .HasOne(f => f.Hotel)
            .WithMany(h => h.FavoriteHotels)
            .HasForeignKey(f => f.HotelId);
    }

}