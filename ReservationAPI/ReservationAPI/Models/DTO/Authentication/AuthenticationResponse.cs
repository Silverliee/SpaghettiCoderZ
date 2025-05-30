namespace ReservationAPI.Models.DTO;

public class AuthenticationResponse
{
    public int UserId { get; set; }
    public UserRole Role { get; set; }
}