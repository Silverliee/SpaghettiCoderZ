namespace ReservationAPI.Models.DTO.Statistics;

public class ParkingGlobalStatisticResponse
{
    public int CurrentAvailableParkingSlots { get; set; }
    public double OccupancyRatePercentage { get; set; }
    public int CanceledBookingsCount { get; set; }
    public DateTime PeakBookingDate { get; set; }
    public int NoShowCount { get; set; }
}