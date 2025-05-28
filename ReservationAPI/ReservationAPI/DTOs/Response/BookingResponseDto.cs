using ReservationAPI.Models;

namespace ReservationAPI.DTOs.Response;

public class BookingResponseDto
{
    public List<ParkingSlot> ParkingSlots { get; set; }
    public List<Booking> Bookings { get; set; }

    public BookingResponseDto(List<ParkingSlot> parkingSlots, List<Booking> bookings)
    {
        ParkingSlots = parkingSlots;
        Bookings = bookings;
    }
}