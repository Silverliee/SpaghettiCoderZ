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
    Task<Booking> CreateBookingBySecretaryAsync(BookingBySecretaryRequest bookingRequest);
    Task<Booking> UpdateBookingAsync(Booking booking);
    Task<Booking> UpdateBookingBySecretaryAsync(BookingBySecretaryRequest bookingRequest);
    Task<bool> DeleteBookingAsync(int id);
    Task<bool> DeleteBookingBySecretaryAsync(int id, int secretaryId);
    Task<bool> CheckinBookingAsync(CheckInRequest checkInRequest);
}