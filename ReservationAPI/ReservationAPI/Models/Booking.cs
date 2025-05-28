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

    public Booking()
    {
        
    }

    public Booking(int id, DateOnly date, ParkingSlot slot, BookingStatus status)
    {
        Id = id;
        Date = date;
        Slot = slot;
        Status = status;
    }
}