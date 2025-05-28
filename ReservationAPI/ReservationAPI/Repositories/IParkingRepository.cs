using ReservationAPI.Models;

namespace ReservationAPI.Repositories;

public interface IParkingRepository
{
    Task<List<ParkingSlot>> GetParkingSlotsAsync();
    Task<ParkingSlot?> GetSlotByIdAsync(int id);
    Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateOnly date);
    Task<ParkingSlot> CreateParkingSlotAsync(ParkingSlot parkingSlot);
    Task<ParkingSlot> UpdateParkingSlotAsync(ParkingSlot parkingSlot);
    Task<bool> DeleteParkingSlotAsync(int id);
}