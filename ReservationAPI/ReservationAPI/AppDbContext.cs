using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ReservationAPI.Models;

namespace ReservationAPI;

public class AppDbContext : DbContext
{
    
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<ParkingSlot> ParkingSlots { get; set; }
    
    public AppDbContext() : base()
    {
        //Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get the user's documents folder
        //print current directory
        Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
        var dbPath = Path.Combine("./db/", "ReservationAPI", "database.sqlite");
        //var dbPath = Path.Combine("resources", "db", "database.sqlite");        
        // Ensure directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
        
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure User entity
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date).IsRequired();
            entity.HasOne(e => e.Slot)
                .WithMany()
                .HasForeignKey(e => e.SlotId)
                .IsRequired();
        });

        // Configure Post entity
        modelBuilder.Entity<ParkingSlot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Column).IsRequired();
            entity.Property(e => e.Row).IsRequired();
            entity.Property(e => e.HasCharger).IsRequired();   
            
        });
    }
}