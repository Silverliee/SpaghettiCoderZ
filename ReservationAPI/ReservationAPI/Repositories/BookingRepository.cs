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

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        dbContext.Bookings.Add(booking);
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(booking);
    }

    public async Task<Booking> UpdateBookingAsync(Booking booking)
    {
        dbContext.Bookings.Update(booking);
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(booking);
    }

    public async Task<bool> DeleteBookingAsync(int id)
    {
        var booking = await dbContext.Bookings.FindAsync(id);
        if (booking == null)
        {
            return await Task.FromResult(false);
        }
        booking.Status = BookingStatus.Cancelled; // Soft delete
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<List<Booking>> GetBookingsByDateAsync(DateOnly date)
    {
        var startOfDay = date.ToDateTime(TimeOnly.MinValue); // 2025-05-30T00:00:00
        var endOfDay = date.ToDateTime(TimeOnly.MaxValue);   // 2025-05-30T23:59:59.9999999

        return await Task.FromResult(
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

    public async Task<bool> CheckinBookingAsync(CheckInRequest checkInRequest)
    {
        var booking = await dbContext.Bookings.FindAsync(checkInRequest.BookingId);
        if (booking == null )
        {
            return await Task.FromResult(false);
        }
        booking.Status = BookingStatus.Completed;
        dbContext.Bookings.Update(booking);
        await dbContext.SaveChangesAsync();
        
        return await Task.FromResult(true);
    }
}