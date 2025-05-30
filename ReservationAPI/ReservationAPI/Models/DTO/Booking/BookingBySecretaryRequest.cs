namespace ReservationAPI.Models.DTO.Booking;

public class BookingBySecretaryRequest
{
    public required int SecretaryId { get; set; }
    public required Models.Booking Booking { get; set; }
}