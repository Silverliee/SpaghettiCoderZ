using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Models.DTO;
using ReservationAPI.Services;

namespace ReservationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticController(IStatisticService statisticService) : ControllerBase
{
    [HttpGet("global")]
    public async Task<ActionResult<ParkingGlobalStatisticResponse>> GetGlobalStatistics()
    {
        try
        {
            var statistics = await statisticService.GetParkingGlobalStatisticAsync();
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error while retrieving global parking statistics : {ex.Message}");
        }
    }

    [HttpGet("global/period")]
    public async Task<ActionResult<ParkingGlobalStatisticResponse>> GetGlobalStatisticsByPeriod(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate > endDate)
                return BadRequest("Start date must be earlier than end date");

            var statistics = await statisticService.GetParkingGlobalStatisticByDateAsync(startDate, endDate);
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error while retrieving global parking statistics for the period: {ex.Message}");
        }
    }
    
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<UserStatisticResponse>> GetUserStatistics(int userId)
    {
        try
        {
            var statistics = await statisticService.GetUserStatisticAsync(userId);
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error while retrieving user statistics: {ex.Message}");
        }
    }
    
    [HttpGet("user/{userId:int}/period")]
    public async Task<ActionResult<UserStatisticResponse>> GetUserStatisticsByPeriod(
        int userId, 
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        try
        {
            if (startDate > endDate)
                return BadRequest("Start date must be earlier than end date");

            var statistics = await statisticService.GetUserStatisticByDateAsync(userId, startDate, endDate);
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error while retrieving user statistics for the period: {ex.Message}");
        }
    }
}