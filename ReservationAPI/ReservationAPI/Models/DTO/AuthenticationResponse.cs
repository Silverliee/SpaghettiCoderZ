namespace ReservationAPI.Models.DTO;

public class AuthenticationResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    
    public string? errorMessages { get; set; }
}