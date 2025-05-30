using ReservationAPI.Middlewares.Communication;
using ReservationAPI.Models;
using ReservationAPI.Models.DTO.Booking;
using ReservationAPI.Repositories;

namespace ReservationAPI.Services;

public class BookingService(IBookingRepository bookingRepository, IUserService userService,IMessaging messaging) : IBookingService
{
    public Task<Booking?> GetBookingByIdAsync(int id)
    {
        return bookingRepository.GetBookingByIdAsync(id);
    }

    public Task<List<Booking>> GetBookingsAsync()
    {
        return bookingRepository.GetBookingsAsync();
    }

    public async Task<List<Booking>> GetBookingsByDateAsync(DateOnly date)
    {
        return await bookingRepository.GetBookingsByDateAsync(date);
    }

    public Task<List<Booking>> GetBookingsByUserIdAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than zero", nameof(userId));
        }

        return bookingRepository.GetBookingsByUserIdAsync(userId);
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        if (booking == null)
        {
            throw new ArgumentNullException(nameof(booking), "Booking cannot be null");
        }

        await ValidateBookingAsync(booking);
        messaging.SendMessage($"New booking created for user {booking.UserId} on {booking.Date}");
        
        return await bookingRepository.CreateBookingAsync(booking);
    }

    private async Task ValidateBookingAsync(Booking booking)
    {
        
        var user = userService.GetUserByIdAsync(booking.UserId).Result;
        var maxBookings = user.Role == UserRole.Manager ? 30 : 5;
        // We retrieve all bookings for the user
        var userBookings = await bookingRepository.GetBookingsByUserIdAsync(booking.UserId);
    
        // Check if the user already has a booking for the same date
        var bookingsOnSameDay = userBookings
            .Where(b => b.Date.Date == booking.Date.Date)
            .ToList();

        if (bookingsOnSameDay.Any())
        {
            throw new InvalidOperationException("User already has a booking for this date");
        }

        // Check if the user has more than 5 active bookings
        var activeBookings = userBookings
            .Where(b => b.Date.Date >= DateTime.Today && b.Status == BookingStatus.Booked)
            .ToList();

        if (activeBookings.Count >= maxBookings)
        {
            throw new InvalidOperationException($"User cannot have more {maxBookings} than active bookings at a time");
        }
    }

    public Task<Booking> UpdateBookingAsync(Booking booking)
    {
        return bookingRepository.UpdateBookingAsync(booking);
    }

    public Task<bool> DeleteBookingAsync(int id)
    {
        return bookingRepository.DeleteBookingAsync(id);
    }

    public Task<bool> CheckinBookingAsync(CheckInRequest checkInRequest)
    {
        if (checkInRequest == null)
        {
            throw new ArgumentNullException(nameof(checkInRequest), "Check-in request cannot be null");
        }
        return bookingRepository.CheckinBookingAsync(checkInRequest);
    }
}