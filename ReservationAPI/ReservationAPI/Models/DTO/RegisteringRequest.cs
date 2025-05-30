using System.ComponentModel.DataAnnotations;

namespace ReservationAPI.Models.DTO;

public class RegisteringRequest
{
    public UserRole Role { get; set; }

    [Required(ErrorMessage = "User Firstname is required")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "User Lastname is required")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        ErrorMessage =
            "Password must be at least 8 characters long, contain at least one letter, one number and one special character")]
    public required string Password { get; set; }
}