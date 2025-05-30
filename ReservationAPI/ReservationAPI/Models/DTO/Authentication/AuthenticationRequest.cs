using System.ComponentModel.DataAnnotations;

namespace ReservationAPI.Models.DTO.Authentication;

public class AuthenticationRequest
{
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string? Email { get; set; }
    public string? Password { get; set; }
}