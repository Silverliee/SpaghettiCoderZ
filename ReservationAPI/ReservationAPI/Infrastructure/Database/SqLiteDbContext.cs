using Microsoft.EntityFrameworkCore;
using ReservationAPI.Models;

namespace ReservationAPI.Infrastructure.Database;

public class SqLiteDbContext : DbContext
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<ParkingSlot> ParkingSlots { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");
        var dbPath = Path.Combine("./Infrastructure/", "Database", "database.sqlite");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.SlotId).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            
        });

        modelBuilder.Entity<ParkingSlot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Column).IsRequired();
            entity.Property(e => e.Row).IsRequired();
            entity.Property(e => e.HasCharger).IsRequired();
            entity.Property(e => e.InMaintenance);
        });
    }
}