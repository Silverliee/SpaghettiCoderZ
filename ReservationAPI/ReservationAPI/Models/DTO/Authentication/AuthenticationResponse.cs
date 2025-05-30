namespace ReservationAPI.Models.DTO.Authentication;

public class AuthenticationResponse
{
    public int UserId { get; set; }
    public UserRole Role { get; set; }
}