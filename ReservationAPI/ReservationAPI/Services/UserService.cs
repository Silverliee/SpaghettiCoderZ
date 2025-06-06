using ReservationAPI.Middlewares.Security;
using ReservationAPI.Models;
using ReservationAPI.Models.DTO;
using ReservationAPI.Models.DTO.Authentication;
using ReservationAPI.Models.DTO.Register;
using ReservationAPI.Repositories;

namespace ReservationAPI.Services;

public class UserService(IUserRepository userRepository, ICryptographer cryptographer) : IUserService
{
    public async Task<RegisteringResponse> RegisterUserAsync(RegisteringRequest registeringRequest)
    {
        try
        {
            var myUser = new User
            {
                FirstName = registeringRequest.FirstName,
                LastName = registeringRequest.LastName,
                Email = registeringRequest.Email,
                Role = registeringRequest.Role,
                Password = cryptographer.Encrypt(registeringRequest.Password)
            };
            var result = await userRepository.RegisterUserAsync(myUser);
            var response = new RegisteringResponse
            {
                IsRegistered = true,
                UserId = result?.Id ?? 0,
            };
            return response;
        }
        catch (Exception e)
        {
            var response = new RegisteringResponse
            {
                IsRegistered = false
            };
            return response;
        }
    }

    public async Task<AuthenticationResponse?> LoginUserAsync(AuthenticationRequest request)
    {
        try
        {
            var user = await userRepository.GetUserByEmailAsync(request.Email!);
            if (user.Password != cryptographer.Encrypt(request.Password!))
            {
                return null;
            }
            var result = new AuthenticationResponse
            {
                UserId = user.Id,
                Role = user.Role
            };
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public Task<User?> GetUserByIdAsync(int userId)
    {
        return userRepository.GetUserByIdAsync(userId);
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return userRepository.GetAllUsersAsync();
    }

    public async Task<RegisteringResponse> RegisterUserForSecretaryAsync(RegisteredBySecretaryRequest registeringRequest)
    {
        try
        {
            var secretary = await userRepository.GetUserByIdAsync(registeringRequest.SecretaryId);
            if (secretary == null || secretary.Role != UserRole.Secretary)
            {
                return new RegisteringResponse
                {
                    IsRegistered = false,
                };
            }
            
            var myUser = new User
            {
                FirstName = registeringRequest.FirstName,
                LastName = registeringRequest.LastName,
                Email = registeringRequest.Email,
                Role = registeringRequest.Role,
                Password = cryptographer.Encrypt(registeringRequest.Password)
            };
            var result = await userRepository.RegisterUserAsync(myUser);
            var response = new RegisteringResponse
            {
                IsRegistered = true,
                UserId = result?.Id ?? 0,
            };
            return response;
        }
        catch (Exception e)
        {
            var response = new RegisteringResponse
            {
                IsRegistered = false
            };
            return response;
        }
    }
}