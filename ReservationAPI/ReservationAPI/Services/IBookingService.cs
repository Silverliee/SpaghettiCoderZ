using ReservationAPI.Models;

namespace ReservationAPI.Services;

public interface IBookingService
{
    // Booking operations
    Task<Booking?> GetBookingByIdAsync(int id);
    Task<List<Booking>> GetBookingsAsync();
    Task<List<Booking>> GetBookingsByDateAsync(DateOnly date);
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<Booking> UpdateBookingAsync(Booking booking);
    Task<bool> DeleteBookingAsync(int id);
}