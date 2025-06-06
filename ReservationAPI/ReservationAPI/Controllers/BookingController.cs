﻿using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Models;
using ReservationAPI.Models.DTO.Booking;
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

    [HttpGet("user/{userId:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<Booking>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        try
        {
            var bookings = await bookingService.GetBookingsByUserIdAsync(userId);
            if (bookings.Count == 0)
            {
                return NotFound($"No bookings found for user with ID {userId}.");
            }
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
    
    [HttpPost("secretary")]
    public async Task<IActionResult> PostBySecretary([FromBody] BookingBySecretaryRequest? bookingRequest)
    {
        if (bookingRequest == null)
        {
            return BadRequest("Booking request cannot be null.");
        }
        try
        {
            var booking = await bookingService.CreateBookingBySecretaryAsync(bookingRequest);
            return Ok(booking);
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
    
    [HttpPut("secretary/{id:int}")]
    public async Task<IActionResult> PutBySecretary(int id, [FromBody] BookingBySecretaryRequest? bookingRequest)
    {
        if (bookingRequest == null)
        {
            return BadRequest("Booking request cannot be null.");
        }
        try
        {
            var existingBooking = await bookingService.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }
            bookingRequest.Booking.Id = id;
            var updatedBooking = await bookingService.UpdateBookingBySecretaryAsync(bookingRequest);
            return Ok(updatedBooking);
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
    
    [HttpDelete("secretary/{id:int}")]
    public async Task<IActionResult> DeleteBySecretary(int id, [FromQuery] int secretaryId)
    {
        try
        {
            var existingBooking = await bookingService.GetBookingByIdAsync(id);
            if (existingBooking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }
            var result = await bookingService.DeleteBookingBySecretaryAsync(id, secretaryId);
            if (!result)
            {
                return BadRequest("Failed to delete booking. Secretary may not have permission.");
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("checkin")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Checkin([FromBody] CheckInRequest checkInRequest)
    {
        if (checkInRequest.BookingId < 0 || checkInRequest.UserId < 0)
        {
            return BadRequest("Invalid check-in request.");
        }
        try
        {
            var result = await bookingService.CheckinBookingAsync(checkInRequest);
            if (!result)
            {
                return BadRequest("Check-in failed. Booking may not exist or is already checked in.");
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}