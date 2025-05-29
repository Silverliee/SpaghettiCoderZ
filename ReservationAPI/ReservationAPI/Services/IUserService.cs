using ReservationAPI.Models;
using ReservationAPI.Models.DTO;

namespace ReservationAPI.Services;

public interface IUserService
{
    public Task<User?> GetUserByIdAsync(int userId);
    public Task<User> GetUserByEmailAsync(string email);
    public Task<User> RegisterUserAsync(User user);
    public Task<AuthenticationResponse?> LoginUserAsync(string email, string password);
    public Task<User> UpdateUserAsync(User user);
    public Task<bool> DeleteUserAsync(int userId);
}