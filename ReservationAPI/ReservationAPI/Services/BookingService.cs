using ReservationAPI.Models;
using ReservationAPI.Repositories;

namespace ReservationAPI.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public Task<Booking?> GetBookingByIdAsync(int id)
    {
        return _bookingRepository.GetBookingByIdAsync(id);
    }

    public Task<List<Booking>> GetBookingsAsync()
    {
        return _bookingRepository.GetBookingsAsync();
    }

    public async Task<List<Booking>> GetBookingsByDateAsync(DateOnly date)
    {

        return await _bookingRepository.GetBookingsByDateAsync(date);
    }

    public Task<Booking> CreateBookingAsync(Booking booking)
    {
        if (booking == null)
        {
            throw new ArgumentNullException(nameof(booking), "Booking cannot be null");
        }

        return _bookingRepository.CreateBookingAsync(booking);
    }

    public Task<Booking> UpdateBookingAsync(Booking booking)
    {
        return _bookingRepository.UpdateBookingAsync(booking);
    }

    public Task<bool> DeleteBookingAsync(int id)
    {
        return _bookingRepository.DeleteBookingAsync(id);
    }

    public async Task<List<ParkingSlot>> GetAllParkingSlotsAsync()
    {
        
        return await _bookingRepository.GetAllParkingSlotsAsync();
    }

    public Task<ParkingSlot?> GetSlotByIdAsync(int id)
    {
        return _bookingRepository.GetSlotByIdAsync(id);
    }

    public Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateOnly date)
    {
        return _bookingRepository.GetAvailableSlotsAsync(date);
    }
    
}