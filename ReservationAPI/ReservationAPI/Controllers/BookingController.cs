using Microsoft.AspNetCore.Mvc;
using ReservationAPI.DTOs.Response;
using ReservationAPI.Models;
using ReservationAPI.Services;

namespace ReservationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        // retourne un ok json avec des fakes data de reservation :
        return Ok(new
        {
            Bookings = new[]
            {
                new { Id = 1, PlaceId = 1, Date = "2023-10-01", User = 1 },
                new { Id = 2, PlaceId = 2, Date = "2023-10-02", User = 2 }
            }
        });
    }

    [HttpGet("byDate")]
    public async Task<ActionResult<BookingResponseDto>> GetByDate([FromQuery] DateTime date)
    {
        List<Booking> bookings; 
        try
        {
            bookings = await _bookingService.GetBookingsByDateAsync(date);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        List<ParkingSlot> parkingSlots;
        try
        {
            parkingSlots = await _bookingService.GetAllParkingSlotsAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        var bookingsResult = new BookingResponseDto(parkingSlots, bookings);
        
        return bookingsResult;
    }

    public async Task<ActionResult> Post([FromQuery] int idSlot, DateTime date)
    {
        return null;
    }
}