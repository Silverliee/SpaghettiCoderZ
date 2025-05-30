using ReservationAPI.Models;
using ReservationAPI.Models.DTO.Booking;

namespace ReservationAPI.Repositories;

public interface IBookingRepository
{
    Task<List<Booking>> GetBookingsAsync();
    Task<Booking?> GetBookingByIdAsync(int id);
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<Booking> UpdateBookingAsync(Booking booking);
    Task<bool> DeleteBookingAsync(int id);
    Task<List<Booking>> GetBookingsByDateAsync(DateOnly date);
    Task<List<Booking>> GetBookingsByUserIdAsync(int userId);
    Task<bool> CheckinBookingAsync(CheckInRequest checkInRequest);
}