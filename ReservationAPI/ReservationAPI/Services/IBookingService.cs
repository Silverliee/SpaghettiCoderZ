using ReservationAPI.Models;

namespace ReservationAPI.Services;

public interface IBookingService
{
    Task<List<Booking>> GetBookingsByDateAsync(DateOnly date);
    Task<List<ParkingSlot>> GetAllParkingSlotsAsync();
}