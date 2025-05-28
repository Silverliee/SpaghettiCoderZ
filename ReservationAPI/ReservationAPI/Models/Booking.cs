namespace ReservationAPI.Models;

public class Booking
{
    public DateOnly Date { get; set; }
    public ParkingSlot Slot { get; set; }
}