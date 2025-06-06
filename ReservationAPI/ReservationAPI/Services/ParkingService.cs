using ReservationAPI.Models;
using ReservationAPI.Repositories;

namespace ReservationAPI.Services;

public class ParkingService(IParkingRepository parkingRepository) : IParkingService
{
    public async Task<List<ParkingSlot>> GetAllParkingSlotsAsync()
    {
        return await parkingRepository.GetParkingSlotsAsync();
    }

    public async Task<ParkingSlot?> GetSlotByIdAsync(int id)
    {
        return await parkingRepository.GetSlotByIdAsync(id);
    }

    public async Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateOnly date)
    {
        return await parkingRepository.GetAvailableSlotsAsync(date);
    }

    public Task AddParkingSlotAsync(ParkingSlot parkingSlot)
    {
        if (parkingSlot == null)
        {
            throw new ArgumentNullException(nameof(parkingSlot), "Parking slot cannot be null.");
        }

        return parkingRepository.CreateParkingSlotAsync(parkingSlot);
    }

    public Task UpdateParkingSlotAsync(ParkingSlot parkingSlot)
    {
        if (parkingSlot == null)
        {
            throw new ArgumentNullException(nameof(parkingSlot), "Parking slot cannot be null.");
        }

        return parkingRepository.UpdateParkingSlotAsync(parkingSlot);
    }

    public Task DeleteParkingSlotAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid parking slot ID.", nameof(id));
        }

        return parkingRepository.DeleteParkingSlotAsync(id);
    }
}