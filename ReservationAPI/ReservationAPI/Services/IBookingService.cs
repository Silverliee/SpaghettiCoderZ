using ReservationAPI.Models;
using ReservationAPI.Models.DTO.Booking;

namespace ReservationAPI.Services;

public interface IBookingService
{
    // Booking operations
    Task<Booking?> GetBookingByIdAsync(int id);
    Task<List<Booking>> GetBookingsAsync();
    Task<List<Booking>> GetBookingsByDateAsync(DateOnly date);
    Task<List<Booking>> GetBookingsByUserIdAsync(int userId);
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<Booking> UpdateBookingAsync(Booking booking);
    Task<bool> DeleteBookingAsync(int id);
    Task<bool> CheckinBookingAsync(CheckInRequest checkInRequest);
}