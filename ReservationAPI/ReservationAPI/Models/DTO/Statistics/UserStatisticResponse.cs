namespace ReservationAPI.Models.DTO;

public class UserStatisticResponse
{
    public int UserId { get; set; }
    public int BookingsCount { get; set; }
    public int CancellationsCount { get; set; }
    public int NoShowsCount { get; set; }
    public DateTime LastBookingDate { get; set; }
}