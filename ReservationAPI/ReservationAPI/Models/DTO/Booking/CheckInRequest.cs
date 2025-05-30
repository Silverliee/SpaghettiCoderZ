namespace ReservationAPI.Models.DTO.Booking;

public class CheckInRequest
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
}