﻿using HotelService.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions
        <AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Host> Hosts { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("DefaultConnection",
                b => b.MigrationsAssembly("HotelService.Db")); 
        }
    }
}