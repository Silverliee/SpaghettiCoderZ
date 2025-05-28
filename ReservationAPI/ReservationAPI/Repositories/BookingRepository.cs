using Microsoft.EntityFrameworkCore;
using ReservationAPI.Infrastructure.Database;
using ReservationAPI.Models;

namespace ReservationAPI.Repositories;

public class BookingRepository(SqLiteDbContext dbContext) : IBookingRepository
{
    public async Task<List<Booking>> GetBookingsAsync()
    {
        return await dbContext.Bookings.OrderBy(u=> u.Date).ToListAsync();
    }

    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        return await dbContext.Bookings.FindAsync(id);
    }

    public Task<Booking> CreateBookingAsync(Booking booking)
    {
        ArgumentNullException.ThrowIfNull(booking);

        dbContext.Bookings.Add(booking);
        dbContext.SaveChanges();
        return Task.FromResult(booking);
    }

    public Task<Booking> UpdateBookingAsync(Booking booking)
    {
        dbContext.Bookings.Update(booking);
        dbContext.SaveChanges();
        return Task.FromResult(booking);
    }

    public Task<bool> DeleteBookingAsync(int id)
    {
        var booking = dbContext.Bookings.Find(id);
        if (booking == null)
        {
            return Task.FromResult(false);
        }
        dbContext.Bookings.Remove(booking);
        dbContext.SaveChanges();
        return Task.FromResult(true);
    }

    public Task<List<Booking>> GetBookingsByDateAsync(DateOnly date)
    {
        return Task.FromResult(dbContext.Bookings.Where(b => b.Date == date.ToDateTime(TimeOnly.MinValue))
            .OrderBy(b => b.Date)
            .ToList());
    }
}