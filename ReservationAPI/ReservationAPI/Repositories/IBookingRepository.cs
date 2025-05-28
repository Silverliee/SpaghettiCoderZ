using ReservationAPI.Models;

namespace ReservationAPI.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetBookingByIdAsync(int id);
    Task<List<Booking>> GetBookingsAsync();
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<Booking> UpdateBookingAsync(Booking booking);
    Task<bool> DeleteBookingAsync(int id);
    Task<List<Booking>> GetBookingsByDateAsync(DateOnly date);
    
    // Additional methods to manage Parking Slots
    Task<ParkingSlot?> GetSlotByIdAsync(int id);
    Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateOnly date);
    Task<List<ParkingSlot>> GetAllParkingSlotsAsync();
}