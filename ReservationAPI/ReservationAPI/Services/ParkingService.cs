using ReservationAPI.Models;
using ReservationAPI.Repositories;

namespace ReservationAPI.Services;

public class ParkingService(IParkingRepository parkingRepository) : IParkingService
{
    public async Task<List<ParkingSlot>> GetAllParkingSlotsAsync()
    {
        return await parkingRepository.GetParkingSlotsAsync();
    }

    public Task<ParkingSlot?> GetSlotByIdAsync(int id)
    {
        return parkingRepository.GetSlotByIdAsync(id);
    }

    public Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateOnly date)
    {
        return parkingRepository.GetAvailableSlotsAsync(date);
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