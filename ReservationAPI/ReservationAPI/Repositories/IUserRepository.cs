using ReservationAPI.Models;

namespace ReservationAPI.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByIdAsync(int userId);
    public Task<User> GetUserByEmailAsync(string email);
    public Task<User> RegisterUserAsync(User user);
    public Task<User> UpdateUserAsync(User user);
    public Task<bool> DeleteUserAsync(int userId);
    
    public Task<List<User>> GetAllUsersAsync();
}