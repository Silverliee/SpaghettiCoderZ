using ReservationAPI.Models;

namespace ReservationAPI.Services;

public interface IParkingService
{
    Task<List<ParkingSlot>> GetAllParkingSlotsAsync();
    Task<ParkingSlot?> GetSlotByIdAsync(int id);
    Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateOnly date);
    Task AddParkingSlotAsync(ParkingSlot parkingSlot);
    Task UpdateParkingSlotAsync(ParkingSlot parkingSlot);
    Task DeleteParkingSlotAsync(int id);
}