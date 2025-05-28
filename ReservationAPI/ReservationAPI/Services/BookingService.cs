using ReservationAPI.Models;

namespace ReservationAPI.Services;

public class BookingService : IBookingService
{
    public async Task<List<Booking>> GetBookingsByDateAsync(DateTime date)
    {
        var parkings = await GetAllParkingSlotsAsync();
        List<Booking> bookings = [];
        for (int i = 0; i < 3; i++)
        {
            bookings.Add(new Booking(i, date, parkings[i], BookingStatus.Booked));
        }

        return bookings;
    }

    public Task<List<ParkingSlot>> GetAllParkingSlotsAsync()
    {
        List<ParkingSlot> slots = [];
        for (int i = 0; i < 3; i++)
        {
            slots.Add(new ParkingSlot(i, "A", i, true));
        }

        return Task.FromResult(slots);
    }
}