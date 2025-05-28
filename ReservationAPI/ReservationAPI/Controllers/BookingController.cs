using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Models;
using ReservationAPI.Services;

namespace ReservationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController(IBookingService bookingService) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Booking>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Get()
    {
        try
        {
            var bookings = await bookingService.GetBookingsAsync();
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    [HttpGet("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Booking), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var booking = await bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }
            return Ok(booking);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("{date:date}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Booking), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Get(DateOnly date)
    {
        try
        {
            var bookings = await bookingService.GetBookingsByDateAsync(date);
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Booking? booking)
    {
        if (booking == null)
        {
            return BadRequest("Booking cannot be null.");
        }
        try
        {
            await bookingService.CreateBookingAsync(booking);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}