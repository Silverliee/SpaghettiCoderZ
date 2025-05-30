using System.ComponentModel.DataAnnotations;

namespace ReservationAPI.Models;

public class User
{
    [Key] public int Id { get; set; }
    public UserRole Role { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}