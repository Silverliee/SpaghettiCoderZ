using ReservationAPI.Models;

namespace ReservationAPI.Services;

public interface IBookingService
{
    Task<List<Booking>> GetBookingsByDateAsync(DateTime date);
    Task<List<ParkingSlot>> GetAllParkingSlotsAsync();
    Task<Booking> CreateBooking();
}