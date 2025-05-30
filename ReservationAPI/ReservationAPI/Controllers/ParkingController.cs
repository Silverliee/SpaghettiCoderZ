using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Models;
using ReservationAPI.Services;

namespace ReservationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ParkingController(IParkingService parkingService) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(List<ParkingSlot>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Get()
    {
        try
        {
            var parkingSlots = await parkingService.GetAllParkingSlotsAsync();
            return Ok(parkingSlots);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Post([FromBody] ParkingSlot? parkingSlot)
    {
        if (parkingSlot == null)
        {
            return BadRequest("Parking slot cannot be null.");
        }
        try
        {
            await parkingService.AddParkingSlotAsync(parkingSlot);
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
    public async Task<IActionResult> Post([FromBody] List<ParkingSlot>? parkingSlots)
    {
        if (parkingSlots == null || parkingSlots.Count == 0)
        {
            return BadRequest("Parking slots cannot be null or empty.");
        }
        try
        {
            foreach (var slot in parkingSlots)
            {
                await parkingService.AddParkingSlotAsync(slot);
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Put([FromBody] ParkingSlot? parkingSlot)
    {
        if (parkingSlot == null)
        {
            return BadRequest("Parking slot cannot be null.");
        }
        try
        {
            await parkingService.UpdateParkingSlotAsync(parkingSlot);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await parkingService.DeleteParkingSlotAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}