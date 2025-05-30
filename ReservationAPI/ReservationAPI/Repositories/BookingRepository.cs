using Microsoft.EntityFrameworkCore;
using ReservationAPI.Infrastructure.Database;
using ReservationAPI.Models;
using ReservationAPI.Models.DTO.Booking;

namespace ReservationAPI.Repositories;

public class BookingRepository(SqLiteDbContext dbContext) : IBookingRepository
{
    public async Task<List<Booking>> GetBookingsAsync()
    {
        return await dbContext.Bookings.OrderBy(u => u.Date).ToListAsync();
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
        booking.Status = BookingStatus.Cancelled; // Soft delete
        dbContext.SaveChanges();
        return Task.FromResult(true);
    }

    public Task<List<Booking>> GetBookingsByDateAsync(DateOnly date)
    {
        var startOfDay = date.ToDateTime(TimeOnly.MinValue); // 2025-05-30T00:00:00
        var endOfDay = date.ToDateTime(TimeOnly.MaxValue);   // 2025-05-30T23:59:59.9999999

        return Task.FromResult(
            dbContext.Bookings
                .Where(b => b.Date >= startOfDay && b.Date <= endOfDay)
                .OrderBy(b => b.Date)
                .ToList()
        );
    }
    
    public async Task<List<Booking>> GetBookingsByUserIdAsync(int userId)
    {
        return await dbContext.Bookings
            .Where(b => b.UserId == userId)
            .OrderBy(b => b.Date)
            .ToListAsync();
    }

    public Task<bool> CheckinBookingAsync(CheckInRequest checkInRequest)
    {
        var booking = dbContext.Bookings.Find(checkInRequest.BookingId);
        if (booking == null || booking.Status != BookingStatus.Booked || booking.UserId != checkInRequest.UserId)
        {
            return Task.FromResult(false);
        }

        booking.Status = BookingStatus.Completed;
        dbContext.Bookings.Update(booking);
        dbContext.SaveChanges();
        
        return Task.FromResult(true);
    }
}