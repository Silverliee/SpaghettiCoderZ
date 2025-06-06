using Microsoft.EntityFrameworkCore;
using ReservationAPI.Models;

namespace ReservationAPI.Infrastructure.Database;

public class SqLiteDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<ParkingSlot> ParkingSlots { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected readonly IConfiguration Configuration = configuration;


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
            entity.Property(e => e.UserId).IsRequired();
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
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Role).IsRequired();
        });
    }
}