using ReservationAPI.Infrastructure.Database;
using ReservationAPI.Models;

namespace ReservationAPI.Repositories;

public class ParkingRepository(SqLiteDbContext dbContext) : IParkingRepository
{
    public Task<List<ParkingSlot>> GetParkingSlotsAsync()
    {
        return Task.FromResult(dbContext.ParkingSlots
            .OrderBy(slot => slot.Row)
            .ThenBy(slot => slot.Column)
            .ToList());
    }

    public Task<ParkingSlot?> GetSlotByIdAsync(int id)
    {
        return dbContext.ParkingSlots.FindAsync(id).AsTask();
    }

   public Task<List<ParkingSlot>> GetAvailableSlotsAsync(DateOnly date)
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
   
       return Task.FromResult(availableSlots);
   }

    public Task<ParkingSlot> CreateParkingSlotAsync(ParkingSlot parkingSlot)
    {
        dbContext.ParkingSlots.Add(parkingSlot);
        dbContext.SaveChanges();
        return Task.FromResult(parkingSlot);
    }

    public Task<ParkingSlot> UpdateParkingSlotAsync(ParkingSlot parkingSlot)
    {
        dbContext.ParkingSlots.Update(parkingSlot);
        dbContext.SaveChanges();
        return Task.FromResult(parkingSlot);
    }

    public Task<bool> DeleteParkingSlotAsync(int id)
    {
        var parkingSlot = dbContext.ParkingSlots.Find(id);
        if (parkingSlot == null)
        {
            return Task.FromResult(false);
        }
        dbContext.ParkingSlots.Remove(parkingSlot);
        dbContext.SaveChanges();
        return Task.FromResult(true);
    }
}