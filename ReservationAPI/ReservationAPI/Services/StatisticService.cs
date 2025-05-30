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

        var today = DateTime.Today;

        // Occupied parking slots for today
        var currentlyOccupied = allBookings.Count(b =>
            b.Date.Date == today &&
            b.Status == BookingStatus.Completed);

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

    public async Task<ParkingGlobalStatisticResponse> GetParkingGlobalStatisticByDateAsync(DateTime startDate,
        DateTime endDate)
    {
        var allBookings = await bookingRepository.GetBookingsAsync();
        var allParkingSlots = await parkingRepository.GetParkingSlotsAsync();

        // Filter bookings within the specified date range
        var filteredBookings = allBookings
            .Where(b => b.Date.Date >= startDate.Date && b.Date.Date <= endDate.Date)
            .ToList();

        // Calculate total slot days in the period
        var totalSlotDays = allParkingSlots.Count * ((endDate - startDate).Days + 1);

        // For each day in the period, count the number of booked slots
        var bookedSlotDays = 0;
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            bookedSlotDays += filteredBookings.Count(b =>
                b.Date.Date == date &&
                b.Status == BookingStatus.Completed);
        }

        var occupancyRate = totalSlotDays > 0
            ? (double)bookedSlotDays / totalSlotDays * 100
            : 0;

        // Canceled bookings in the period
        var canceledBookings = filteredBookings.Count(b => b.Status == BookingStatus.Cancelled);

        // Most booked date in the period
        var peakDate = filteredBookings.Any()
            ? filteredBookings
                .GroupBy(b => b.Date.Date)
                .OrderByDescending(g => g.Count())
                .First().Key
            : startDate;

        // No-shows in the period
        var today = DateTime.Today;
        var noShows = filteredBookings.Count(b =>
            b.Status == BookingStatus.NoShow &&
            b.Date.Date < today);

        return new ParkingGlobalStatisticResponse
        {
            CurrentAvailableParkingSlots = allParkingSlots.Count,
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