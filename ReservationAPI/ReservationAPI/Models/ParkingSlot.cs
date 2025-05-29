using System.ComponentModel.DataAnnotations;

namespace ReservationAPI.Models;

public class ParkingSlot
{
    [Key]
    public int Id { get; set; }
    public string Row { get; set; }
    public int Column { get; set; }
    public bool HasCharger { get; set; }
    public bool InMaintenance { get; set; }
}