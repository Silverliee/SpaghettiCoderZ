namespace ReservationAPI.Models.DTO;

public class AuthenticationResponse
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}