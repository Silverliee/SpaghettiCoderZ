using Microsoft.EntityFrameworkCore;
using ReservationAPI.Models;

namespace ReservationAPI.Repositories;

public class BookingRepository(AppDbContext dbContext) : IBookingRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        return await _dbContext.Bookings.FindAsync(id);
    }

    public async Task<List<Booking>> GetBookingsAsync()
    {
        return await _dbContext.Bookings.OrderBy(u=> u.Date).ToListAsync();
    }

    public Task<Booking> CreateBookingAsync(Booking booking)
    {
        ArgumentNullException.ThrowIfNull(booking);

        _dbContext.Bookings.Add(booking);
        _dbContext.SaveChanges();
        return Task.FromResult(booking);
    }

    public Task<Booking> UpdateBookingAsync(Booking booking)
    {
        if (booking == null)
        {
            throw new ArgumentNullException(nameof(booking));
        }

        _dbContext.Bookings.Update(booking);
        _dbContext.SaveChanges();
        return Task.FromResult(booking);
    }

    public Task<bool> DeleteBookingAsync(int id)
    {
        var booking = _dbContext.Bookings.Find(id);
        if (booking == null)
        {
            return Task.FromResult(false);
        }

        _dbContext.Bookings.Remove(booking);
        _dbContext.SaveChanges();
        return Task.FromResult(true);
    }

    public Task<List<Booking>> GetBookingsByDateAsync(DateOnly date)
    {
        return Task.FromResult(_dbContext.Bookings.Where(b => b.Date == date.ToDateTime(TimeOnly.MinValue))
            .OrderBy(b => b.Date)
            .ToList());
    }

    public Task<List<Booking>> GetBookingsByDateAsync(DateTime date)
    {
        return Task.FromResult(_dbContext.Bookings.Where(b => b.Date == date)
            .OrderBy(b => b.Date)
            .ToList());
    }

    public Task<ParkingSlot?> GetSlotByIdAsync(int id)
    {
        return _dbContext.ParkingSlots.FindAsync(id).AsTask();
    }

    public Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateOnly date)
    {
        return Task.FromResult(_dbContext.ParkingSlots
            .Where(slot => !_dbContext.Bookings.Any(b => b.Date == date.ToDateTime(TimeOnly.MinValue) && b.Slot.Id == slot.Id))
            .OrderBy(slot => slot.Row)
            .ThenBy(slot => slot.Column)
            .ToList());
    }

    public Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateTime date)
    {
        return Task.FromResult(_dbContext.ParkingSlots
            .Where(slot => !_dbContext.Bookings.Any(b => b.Date == date && b.Slot.Id == slot.Id))
            .OrderBy(slot => slot.Row)
            .ThenBy(slot => slot.Column)
            .ToList());
    }

    public Task<List<ParkingSlot>> GetAllParkingSlotsAsync()
    {
        return _dbContext.ParkingSlots.OrderBy(slot => slot.Row).ThenBy(slot => slot.Column).ToListAsync();
    }
}