namespace ReservationAPI.Models;

public class ParkingSlot
{
    public int Id { get; set; }
    public string Row { get; set; }
    public int Column { get; set; }
    public bool HasCharger { get; set; }
    
}