namespace ReservationAPI.Models;

public class Booking
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int SlotId { get; set; }
    public BookingStatus Status { get; set; }
}