using ReservationAPI.Middlewares.Communication;
using ReservationAPI.Models;
using ReservationAPI.Models.DTO.Booking;
using ReservationAPI.Repositories;

namespace ReservationAPI.Services;

public class BookingService(IBookingRepository bookingRepository, IMessaging messaging, IUserRepository userRepository) : IBookingService
{
    public async Task<Booking?> GetBookingByIdAsync(int id)
    {
        return await bookingRepository.GetBookingByIdAsync(id);
    }

    public async Task<List<Booking>> GetBookingsAsync()
    {
        return await bookingRepository.GetBookingsAsync();
    }

    public async Task<List<Booking>> GetBookingsByDateAsync(DateOnly date)
    {
        return await bookingRepository.GetBookingsByDateAsync(date);
    }

    public async Task<List<Booking>> GetBookingsByUserIdAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than zero", nameof(userId));
        }

        return await bookingRepository.GetBookingsByUserIdAsync(userId);
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

    public async Task<Booking> CreateBookingBySecretaryAsync(BookingBySecretaryRequest bookingRequest)
    {
        var secretary = await userRepository.GetUserByIdAsync(bookingRequest.SecretaryId);
        if (secretary == null || secretary.Role != UserRole.Secretary)
        {
            throw new ArgumentException("Secretary not found", nameof(bookingRequest.SecretaryId));
        }
         // Assuming secretary is the user making the booking
        return await CreateBookingAsync(bookingRequest.Booking);
    }

    private async Task ValidateBookingAsync(Booking booking)
    {
        
        var user = userRepository.GetUserByIdAsync(booking.UserId).Result;
        var maxBookings = user!.Role == UserRole.Manager ? 30 : 5;
        // We retrieve all bookings for the user
        var userBookings = await bookingRepository.GetBookingsByUserIdAsync(booking.UserId);
    
        // Check if the user already has a booking for the same date
        var bookingsOnSameDay = userBookings
            .Where(b => b.Date.Date == booking.Date.Date)
            .ToList();

        if (bookingsOnSameDay.Count != 0)
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

    public async Task<Booking> UpdateBookingAsync(Booking booking)
    {
        return await bookingRepository.UpdateBookingAsync(booking);
    }

    public async Task<bool> DeleteBookingAsync(int id)
    {
        return await bookingRepository.DeleteBookingAsync(id);
    }

    public async Task<bool> CheckinBookingAsync(CheckInRequest checkInRequest)
    {
        if (checkInRequest == null)
        {
            throw new ArgumentNullException(nameof(checkInRequest), "Check-in request cannot be null");
        }
        
        var booking = await bookingRepository.GetBookingByIdAsync(checkInRequest.BookingId);
        if (booking is not { Status: BookingStatus.Booked } || booking.UserId != checkInRequest.UserId)
        {
            return await Task.FromResult(false);
        }
        
        return await bookingRepository.CheckinBookingAsync(checkInRequest);
    }
}