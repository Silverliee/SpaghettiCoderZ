using ReservationAPI.Models;
using ReservationAPI.Repositories;

namespace ReservationAPI.Services;

public class BookingService(IBookingRepository bookingRepository) : IBookingService
{
    public Task<Booking?> GetBookingByIdAsync(int id)
    {
        return bookingRepository.GetBookingByIdAsync(id);
    }

    public Task<List<Booking>> GetBookingsAsync()
    {
        return bookingRepository.GetBookingsAsync();
    }

    public async Task<List<Booking>> GetBookingsByDateAsync(DateOnly date)
    {
        return await bookingRepository.GetBookingsByDateAsync(date);
    }

    public Task<Booking> CreateBookingAsync(Booking booking)
    {
        if (booking == null)
        {
            throw new ArgumentNullException(nameof(booking), "Booking cannot be null");
        }

        return bookingRepository.CreateBookingAsync(booking);
    }

    public Task<Booking> UpdateBookingAsync(Booking booking)
    {
        return bookingRepository.UpdateBookingAsync(booking);
    }

    public Task<bool> DeleteBookingAsync(int id)
    {
        return bookingRepository.DeleteBookingAsync(id);
    }
}