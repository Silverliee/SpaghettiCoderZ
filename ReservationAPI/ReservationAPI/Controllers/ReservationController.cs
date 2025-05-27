using Microsoft.AspNetCore.Mvc;

namespace ReservationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // retourne un ok json avec des fakes data de reservation :
        return Ok(new
        {
            Reservations = new[]
            {
                new { Id = 1, PlaceId = 1, Date = "2023-10-01", User = 1 },
                new { Id = 2, PlaceId = 2, Date = "2023-10-02", User = 2 }
            }
        });
    }
}