using System.ComponentModel.DataAnnotations;

namespace ReservationAPI.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int SlotId { get; set; }
    public int UserId { get; set; }
    public BookingStatus Status { get; set; }
}