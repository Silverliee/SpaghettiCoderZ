using ReservationAPI.Models;
using ReservationAPI.Models.DTO;
using ReservationAPI.Models.DTO.Statistics;
using ReservationAPI.Repositories;

namespace ReservationAPI.Services;

public class StatisticService(IBookingRepository bookingRepository, IParkingRepository parkingRepository)
    : IStatisticService
{
    public async Task<ParkingGlobalStatisticResponse> GetParkingGlobalStatisticAsync()
    {
        var allBookings = await bookingRepository.GetBookingsAsync();
        var allParkingSlots = await parkingRepository.GetParkingSlotsAsync();

        var today = DateTime.UtcNow.Date;

        // Occupied parking slots for today
        var currentlyOccupied = allBookings.Count(b =>
            b.Date.Date == today && 
            b.Status is BookingStatus.Booked or BookingStatus.Completed); 

        var currentAvailable = allParkingSlots.Count - currentlyOccupied;

        // occupation rate 
        var occupancyRate = allParkingSlots.Count > 0
            ? (double)currentlyOccupied / allParkingSlots.Count * 100
            : 0;

        // Canaceled bookings
        var canceledBookings = allBookings.Count(b => b.Status == BookingStatus.Cancelled);

        // The most booked date
        var peakDate = allBookings
            .GroupBy(b => b.Date.Date)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault()?.Key ?? DateTime.Today;

        // No-shows 
        var noShows = allBookings.Count(b =>
            b.Status == BookingStatus.NoShow &&
            b.Date.Date < today);

        return new ParkingGlobalStatisticResponse
        {
            CurrentAvailableParkingSlots = currentAvailable,
            OccupancyRatePercentage = occupancyRate,
            CanceledBookingsCount = canceledBookings,
            PeakBookingDate = peakDate,
            NoShowCount = noShows
        };
    }

    public async Task<UserStatisticResponse> GetUserStatisticAsync(int userId)
    {
        var userBookings = await bookingRepository.GetBookingsByUserIdAsync(userId);
        if (userBookings == null || !userBookings.Any())
            throw new KeyNotFoundException($"No bookings found for user with id {userId}");
        
        return CalculateUserStatistics(userId, userBookings);
    }

    public async Task<UserStatisticResponse> GetUserStatisticByDateAsync(int userId, DateTime startDate, DateTime endDate)
    {
        var userBookings = await bookingRepository.GetBookingsByUserIdAsync(userId);
        if (userBookings == null || !userBookings.Any())
            throw new KeyNotFoundException($"No bookings found for user with id {userId}");

        // Filter bookings within the specified date range
        var filteredBookings = userBookings
            .Where(b => b.Date.Date >= startDate.Date && b.Date.Date <= endDate.Date)
            .ToList();

        if (!filteredBookings.Any())
            return GetUserStatisticsWithNoBookings(userId);
            
        return CalculateUserStatistics(userId, filteredBookings);
    }

    private static UserStatisticResponse GetUserStatisticsWithNoBookings(int userId)
    {
        return new UserStatisticResponse
        {
            UserId = userId,
            BookingsCount = 0,
            CancellationsCount = 0,
            NoShowsCount = 0,
            LastBookingDate = DateTime.MinValue
        };
    }
    
    private static UserStatisticResponse CalculateUserStatistics(int userId, List<Booking> bookings)
    {
        return new UserStatisticResponse
        {
            UserId = userId,
            BookingsCount = bookings.Count,
            CancellationsCount = bookings.Count(b => b.Status == BookingStatus.Cancelled),
            NoShowsCount = bookings.Count(b => b.Status == BookingStatus.NoShow),
            LastBookingDate = bookings.OrderByDescending(b => b.Date).First().Date
        };
    }
}