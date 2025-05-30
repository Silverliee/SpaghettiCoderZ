using ReservationAPI.Models;
using ReservationAPI.Models.DTO.Authentication;
using ReservationAPI.Models.DTO.Register;

namespace ReservationAPI.Services;

public interface IUserService
{
    public Task<RegisteringResponse> RegisterUserAsync(RegisteringRequest registeringRequest);
    public Task<AuthenticationResponse?> LoginUserAsync(AuthenticationRequest request);
    public Task<User?> GetUserByIdAsync(int userId);
    public Task<List<User>> GetAllUsersAsync();
    public Task<RegisteringResponse> RegisterUserForSecretaryAsync(RegisteredBySecretaryRequest registeringRequest);
}