using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ReservationAPI.Models;
using ReservationAPI.Models.DTO;
using ReservationAPI.Repositories;
using ReservationAPI.Security;

namespace ReservationAPI.Services;

public class UserService(IConfiguration configuration ,IUserRepository userRepository, ICryptographer cryptographer, UserManager<IdentityUser> userManager) : IUserService
{
    private const int ExpirationMinutes = 10;
    
    public Task<User?> GetUserByIdAsync(int userId)
    {
        return userRepository.GetUserByIdAsync(userId);
    }

    public Task<User> GetUserByEmailAsync(string email)
    {
        return userRepository.GetUserByEmailAsync(email);
    }

    public Task<User> RegisterUserAsync(User user)
    {
        var userName = user.FirstName + user.LastName;
        userManager.CreateAsync(new IdentityUser() { UserName = userName, Email = user.Email }, user.Password);
        return userRepository.RegisterUserAsync(user);
    }

    public async Task<AuthenticationResponse?> LoginUserAsync(string email, string password)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var isPasswordValid = await userManager.CheckPasswordAsync(user,password);
            return !isPasswordValid ? null : CreateToken(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }

    public Task<User> UpdateUserAsync(User user)
    {
        return userRepository.UpdateUserAsync(user);
    }

    public Task<bool> DeleteUserAsync(int userId)
    {
        return userRepository.DeleteUserAsync(userId);
    }

    private AuthenticationResponse CreateToken(IdentityUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);

        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return new AuthenticationResponse
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = expiration
        };
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private IEnumerable<Claim> CreateClaims(IdentityUser user) =>
    [
        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
    ];

    private SigningCredentials CreateSigningCredentials() =>
        new(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
            ),
            SecurityAlgorithms.HmacSha256
        );
}