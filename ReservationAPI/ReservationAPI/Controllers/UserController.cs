using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Models.DTO;
using ReservationAPI.Models.DTO.Authentication;
using ReservationAPI.Models.DTO.Register;
using ReservationAPI.Services;

namespace ReservationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        try
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisteringRequest registeringRequest)
    {
        if (string.IsNullOrEmpty(registeringRequest.Email) || string.IsNullOrEmpty(registeringRequest.Password))
        {
            return BadRequest("User data is invalid");
        }

        var registeringResponse = await userService.RegisterUserAsync(registeringRequest);

        if (!registeringResponse.IsRegistered)
        {
            return BadRequest("User registration failed");
        }

        return Ok(registeringResponse);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthenticationRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Email and password are required");
        }

        var loggedInUser = await userService.LoginUserAsync(request);
        if (loggedInUser == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(loggedInUser);
    }
    
    [HttpPost("register/secretary")]
    public async Task<IActionResult> RegisterForSecretary(RegisteredBySecretaryRequest registeringRequest)
    {
        if (string.IsNullOrEmpty(registeringRequest.Email) || string.IsNullOrEmpty(registeringRequest.Password))
        {
            return BadRequest("User data is invalid");
        }

        var registeringResponse = await userService.RegisterUserForSecretaryAsync(registeringRequest);

        if (!registeringResponse.IsRegistered)
        {
            return BadRequest("User registration failed");
        }
        return Ok(registeringResponse);
    }
}