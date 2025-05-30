using ReservationAPI.Infrastructure.Database;
using ReservationAPI.Models;

namespace ReservationAPI.Repositories;

public class ParkingRepository(SqLiteDbContext dbContext) : IParkingRepository
{
    public async Task<List<ParkingSlot>> GetParkingSlotsAsync()
    {
        return await Task.FromResult(dbContext.ParkingSlots
            .OrderBy(slot => slot.Row)
            .ThenBy(slot => slot.Column)
            .ToList());
    }

    public async Task<ParkingSlot?> GetSlotByIdAsync(int id)
    {
        return await dbContext.ParkingSlots.FindAsync(id).AsTask();
    }

   public async Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateOnly date)
   {
       var reservedSlotIds = dbContext.Bookings
           .Where(r => DateOnly.FromDateTime(r.Date) == date)
           .Select(r => r.SlotId)
           .ToList();
   
       var availableSlots = dbContext.ParkingSlots
           .Where(slot => !reservedSlotIds.Contains(slot.Id))
           .OrderBy(slot => slot.Row)
           .ThenBy(slot => slot.Column)
           .ToList();
   
       return await Task.FromResult(availableSlots);
   }

    public async Task<ParkingSlot> CreateParkingSlotAsync(ParkingSlot parkingSlot)
    {
        dbContext.ParkingSlots.Add(parkingSlot);
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(parkingSlot);
    }

    public async Task<ParkingSlot> UpdateParkingSlotAsync(ParkingSlot parkingSlot)
    {
        dbContext.ParkingSlots.Update(parkingSlot);
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(parkingSlot);
    }

    public async Task<bool> DeleteParkingSlotAsync(int id)
    {
        var parkingSlot = await dbContext.ParkingSlots.FindAsync(id);
        if (parkingSlot == null)
        {
            return await Task.FromResult(false);
        }
        dbContext.ParkingSlots.Remove(parkingSlot);
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(true);
    }
}