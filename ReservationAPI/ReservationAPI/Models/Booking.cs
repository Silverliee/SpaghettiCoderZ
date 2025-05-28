using System;

namespace ReservationAPI.Models;

public enum BookingStatus
{
    Booked,
    Cancelled,
    Completed
}

public class Booking
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public ParkingSlot Slot { get; set; }
    public BookingStatus Status { get; set; }
}