using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ReservationAPI.Middlewares.Authentication;

public class AuthenticationMiddleware(IConfiguration configuration)
{
    public string GenerateJwtToken(int userId)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
            var credential = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [new Claim(ClaimTypes.NameIdentifier, userId.ToString())]
                ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credential,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(
                handler.CreateToken(tokenDescriptor)
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}