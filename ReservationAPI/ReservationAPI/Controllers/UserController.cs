using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Models;
using ReservationAPI.Services;

namespace ReservationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User? user)
    {
        if (user == null)
        {
            return BadRequest("User data is required");
        }

        var createdUser = await userService.RegisterUserAsync(user);
        return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return BadRequest("Email and password are required");
        }

        var loggedInUser = await userService.LoginUserAsync(email, password);
        if (loggedInUser == null)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(loggedInUser);
    }
}