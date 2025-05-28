namespace ReservationAPI.Models;

public class ParkingSlot
{
    public int Id { get; set; }
    public string Row { get; set; }
    public int Column { get; set; }
    public bool HasCharger { get; set; }
    
    public ParkingSlot() {}

    public ParkingSlot(int id, string row, int column, bool hasCharger)
    {
        Id = id;
        Row = row;
        Column = column;
        HasCharger = hasCharger;
    }
}