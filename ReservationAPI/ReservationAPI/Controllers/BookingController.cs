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
    
    [HttpGet("date/{date}")]
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
    
    [HttpPost("bulk")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Post([FromBody] List<Booking>? bookings)
    {
        if (bookings == null || bookings.Count == 0)
        {
            return BadRequest("Bookings cannot be null or empty.");
        }
        try
        {
            foreach (var booking in bookings)
            {
                await bookingService.CreateBookingAsync(booking);
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Booking? booking)
    {
        if (booking == null)
        {
            return BadRequest("Booking cannot be null.");
        }
        try
        {
            var existingBooking = await bookingService.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }
            booking.Id = id; // Ensure the ID is set for the update
            await bookingService.UpdateBookingAsync(booking);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var existingBooking = await bookingService.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }
            await bookingService.DeleteBookingAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}