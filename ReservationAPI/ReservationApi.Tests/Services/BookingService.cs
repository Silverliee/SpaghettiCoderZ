using Moq;
using ReservationAPI.Middlewares.Communication;
using ReservationAPI.Models;
using ReservationAPI.Models.DTO.Booking;
using ReservationAPI.Repositories;
using ReservationAPI.Services;

namespace ReservationAPI.Tests.Services;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockBookingRepository;
    private readonly Mock<IMessaging> _mockMessaging;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _mockBookingRepository = new Mock<IBookingRepository>();
        _mockMessaging = new Mock<IMessaging>();
        _mockUserRepository = new Mock<IUserRepository>();
        _bookingService = new BookingService(
            _mockBookingRepository.Object,
            _mockMessaging.Object,
            _mockUserRepository.Object);
    }

    #region GetBookingByIdAsync Tests

    [Fact]
    public async Task GetBookingByIdAsync_ValidId_ReturnsBooking()
    {
        // Arrange
        var bookingId = 1;
        var expectedBooking = new Booking
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.Today,
            Status = BookingStatus.Booked
        };

        _mockBookingRepository.Setup(r => r.GetBookingByIdAsync(bookingId))
            .ReturnsAsync(expectedBooking);

        // Act
        var result = await _bookingService.GetBookingByIdAsync(bookingId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBooking.Id, result.Id);
        Assert.Equal(expectedBooking.UserId, result.UserId);
        Assert.Equal(expectedBooking.Date, result.Date);
        Assert.Equal(expectedBooking.Status, result.Status);
    }

    [Fact]
    public async Task GetBookingByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        var bookingId = 999;
        _mockBookingRepository.Setup(r => r.GetBookingByIdAsync(bookingId))
            .ReturnsAsync((Booking?)null);

        // Act
        var result = await _bookingService.GetBookingByIdAsync(bookingId);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetBookingsAsync Tests

    [Fact]
    public async Task GetBookingsAsync_ReturnsAllBookings()
    {
        // Arrange
        var expectedBookings = new List<Booking>
        {
            new() { Id = 1, UserId = 1, Date = DateTime.Today, Status = BookingStatus.Booked },
            new() { Id = 2, UserId = 2, Date = DateTime.Today.AddDays(1), Status = BookingStatus.Completed }
        };

        _mockBookingRepository.Setup(r => r.GetBookingsAsync())
            .ReturnsAsync(expectedBookings);

        // Act
        var result = await _bookingService.GetBookingsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(expectedBookings, result);
    }

    #endregion

    #region GetBookingsByDateAsync Tests

    [Fact]
    public async Task GetBookingsByDateAsync_ValidDate_ReturnsBookingsForDate()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var expectedBookings = new List<Booking>
        {
            new() { Id = 1, UserId = 1, Date = DateTime.Today, Status = BookingStatus.Booked },
            new() { Id = 2, UserId = 2, Date = DateTime.Today, Status = BookingStatus.Completed }
        };

        _mockBookingRepository.Setup(r => r.GetBookingsByDateAsync(date))
            .ReturnsAsync(expectedBookings);

        // Act
        var result = await _bookingService.GetBookingsByDateAsync(date);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, booking => Assert.Equal(DateTime.Today.Date, booking.Date.Date));
    }

    #endregion

    #region GetBookingsByUserIdAsync Tests

    [Fact]
    public async Task GetBookingsByUserIdAsync_ValidUserId_ReturnsUserBookings()
    {
        // Arrange
        var userId = 1;
        var expectedBookings = new List<Booking>
        {
            new() { Id = 1, UserId = 1, Date = DateTime.Today, Status = BookingStatus.Booked },
            new() { Id = 2, UserId = 1, Date = DateTime.Today.AddDays(1), Status = BookingStatus.Completed }
        };

        _mockBookingRepository.Setup(r => r.GetBookingsByUserIdAsync(userId))
            .ReturnsAsync(expectedBookings);

        // Act
        var result = await _bookingService.GetBookingsByUserIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, booking => Assert.Equal(userId, booking.UserId));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task GetBookingsByUserIdAsync_InvalidUserId_ThrowsArgumentException(int invalidUserId)
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _bookingService.GetBookingsByUserIdAsync(invalidUserId));
        
        Assert.Equal("User ID must be greater than zero (Parameter 'userId')", exception.Message);
    }

    #endregion

    #region CreateBookingAsync Tests

    [Fact]
    public async Task CreateBookingAsync_ValidBooking_CreatesBookingSuccessfully()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = 1,
            Date = DateTime.Today.AddDays(1),
            Status = BookingStatus.Booked
        };

        var user = new User
        {
            Id = 1,
            Role = UserRole.Employee,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var existingBookings = new List<Booking>();
        var createdBooking = new Booking
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.Today.AddDays(1),
            Status = BookingStatus.Booked
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
        _mockBookingRepository.Setup(r => r.GetBookingsByUserIdAsync(1)).ReturnsAsync(existingBookings);
        _mockBookingRepository.Setup(r => r.CreateBookingAsync(booking)).ReturnsAsync(createdBooking);

        // Act
        var result = await _bookingService.CreateBookingAsync(booking);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdBooking.Id, result.Id);
        
        _mockMessaging.Verify(m => m.SendMessage($"New booking created for user {booking.UserId} on {booking.Date}"), Times.Once);
        _mockBookingRepository.Verify(r => r.CreateBookingAsync(booking), Times.Once);
    }

    [Fact]
    public async Task CreateBookingAsync_NullBooking_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _bookingService.CreateBookingAsync(null!));
        
        Assert.Equal("Booking cannot be null (Parameter 'booking')", exception.Message);
    }

    [Fact]
    public async Task CreateBookingAsync_UserAlreadyHasBookingOnSameDate_ThrowsInvalidOperationException()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = 1,
            Date = DateTime.Today,
            Status = BookingStatus.Booked
        };

        var user = new User
        {
            Id = 1,
            Role = UserRole.Employee,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var existingBookings = new List<Booking>
        {
            new() { Id = 1, UserId = 1, Date = DateTime.Today, Status = BookingStatus.Booked }
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
        _mockBookingRepository.Setup(r => r.GetBookingsByUserIdAsync(1)).ReturnsAsync(existingBookings);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _bookingService.CreateBookingAsync(booking));
        
        Assert.Equal("User already has a booking for this date", exception.Message);
    }

    [Fact]
    public async Task CreateBookingAsync_EmployeeHasMaxActiveBookings_ThrowsInvalidOperationException()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = 1,
            Date = DateTime.Today.AddDays(10),
            Status = BookingStatus.Booked
        };

        var user = new User
        {
            Id = 1,
            Role = UserRole.Employee,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        // Create 5 active bookings (max for Employee)
        var existingBookings = new List<Booking>();
        for (int i = 1; i <= 5; i++)
        {
            existingBookings.Add(new Booking
            {
                Id = i,
                UserId = 1,
                Date = DateTime.Today.AddDays(i),
                Status = BookingStatus.Booked
            });
        }

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
        _mockBookingRepository.Setup(r => r.GetBookingsByUserIdAsync(1)).ReturnsAsync(existingBookings);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _bookingService.CreateBookingAsync(booking));
        
        Assert.Equal("User cannot have more 5 than active bookings at a time", exception.Message);
    }

    [Fact]
    public async Task CreateBookingAsync_ManagerHasMaxActiveBookings_ThrowsInvalidOperationException()
    {
        // Arrange
        var booking = new Booking
        {
            UserId = 1,
            Date = DateTime.Today.AddDays(31),
            Status = BookingStatus.Booked
        };

        var user = new User
        {
            Id = 1,
            Role = UserRole.Manager,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        // Create 30 active bookings (max for Manager)
        var existingBookings = new List<Booking>();
        for (int i = 1; i <= 30; i++)
        {
            existingBookings.Add(new Booking
            {
                Id = i,
                UserId = 1,
                Date = DateTime.Today.AddDays(i),
                Status = BookingStatus.Booked
            });
        }

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
        _mockBookingRepository.Setup(r => r.GetBookingsByUserIdAsync(1)).ReturnsAsync(existingBookings);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _bookingService.CreateBookingAsync(booking));
        
        Assert.Equal("User cannot have more 30 than active bookings at a time", exception.Message);
    }

    #endregion

    #region CreateBookingBySecretaryAsync Tests

    [Fact]
    public async Task CreateBookingBySecretaryAsync_ValidSecretary_CreatesBooking()
    {
        // Arrange
        var secretaryUser = new User
        {
            Id = 1,
            Role = UserRole.Secretary,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var targetUser = new User
        {
            Id = 2,
            Role = UserRole.Employee,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var booking = new Booking
        {
            UserId = 2,
            Date = DateTime.Today.AddDays(1),
            Status = BookingStatus.Booked
        };

        var request = new BookingBySecretaryRequest
        {
            SecretaryId = 1,
            Booking = booking
        };

        var createdBooking = new Booking
        {
            Id = 1,
            UserId = 2,
            Date = DateTime.Today.AddDays(1),
            Status = BookingStatus.Booked
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(secretaryUser);
        _mockUserRepository.Setup(r => r.GetUserByIdAsync(2)).ReturnsAsync(targetUser);
        _mockBookingRepository.Setup(r => r.GetBookingsByUserIdAsync(2)).ReturnsAsync(new List<Booking>());
        _mockBookingRepository.Setup(r => r.CreateBookingAsync(booking)).ReturnsAsync(createdBooking);

        // Act
        var result = await _bookingService.CreateBookingBySecretaryAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdBooking.Id, result.Id);
        _mockMessaging.Verify(m => m.SendMessage($"New booking created for user {booking.UserId} on {booking.Date}"), Times.Once);
    }

    [Fact]
    public async Task CreateBookingBySecretaryAsync_SecretaryNotFound_ThrowsArgumentException()
    {
        // Arrange
        var request = new BookingBySecretaryRequest
        {
            SecretaryId = 999,
            Booking = new Booking { UserId = 2, Date = DateTime.Today.AddDays(1) }
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(999)).ReturnsAsync((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _bookingService.CreateBookingBySecretaryAsync(request));
        
        Assert.Equal("Secretary not found (Parameter 'SecretaryId')", exception.Message);
    }

    [Fact]
    public async Task CreateBookingBySecretaryAsync_UserIsNotSecretary_ThrowsArgumentException()
    {
        // Arrange
        var nonSecretaryUser = new User
        {
            Id = 1,
            Role = UserRole.Employee,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var request = new BookingBySecretaryRequest
        {
            SecretaryId = 1,
            Booking = new Booking { UserId = 2, Date = DateTime.Today.AddDays(1) }
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(nonSecretaryUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _bookingService.CreateBookingBySecretaryAsync(request));
        
        Assert.Equal("Secretary not found (Parameter 'SecretaryId')", exception.Message);
    }

    #endregion

    #region UpdateBookingAsync Tests

    [Fact]
    public async Task UpdateBookingAsync_ValidBooking_UpdatesBooking()
    {
        // Arrange
        var booking = new Booking
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.Today,
            Status = BookingStatus.Completed
        };

        var updatedBooking = new Booking
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.Today,
            Status = BookingStatus.Completed
        };

        _mockBookingRepository.Setup(r => r.UpdateBookingAsync(booking)).ReturnsAsync(updatedBooking);

        // Act
        var result = await _bookingService.UpdateBookingAsync(booking);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedBooking.Id, result.Id);
        Assert.Equal(updatedBooking.Status, result.Status);
    }

    #endregion

    #region UpdateBookingBySecretaryAsync Tests

    [Fact]
    public async Task UpdateBookingBySecretaryAsync_ValidSecretary_UpdatesBooking()
    {
        // Arrange
        var secretaryUser = new User
        {
            Id = 1,
            Role = UserRole.Secretary,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var booking = new Booking
        {
            Id = 1,
            UserId = 2,
            Date = DateTime.Today,
            Status = BookingStatus.Completed
        };

        var request = new BookingBySecretaryRequest
        {
            SecretaryId = 1,
            Booking = booking
        };

        var updatedBooking = new Booking
        {
            Id = 1,
            UserId = 2,
            Date = DateTime.Today,
            Status = BookingStatus.Completed
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(secretaryUser);
        _mockBookingRepository.Setup(r => r.UpdateBookingAsync(booking)).ReturnsAsync(updatedBooking);

        // Act
        var result = await _bookingService.UpdateBookingBySecretaryAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedBooking.Id, result.Id);
    }

    [Fact]
    public async Task UpdateBookingBySecretaryAsync_UserIsNotSecretary_ThrowsArgumentException()
    {
        // Arrange
        var nonSecretaryUser = new User
        {
            Id = 1,
            Role = UserRole.Employee,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var request = new BookingBySecretaryRequest
        {
            SecretaryId = 1,
            Booking = new Booking { Id = 1, UserId = 2 }
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(nonSecretaryUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _bookingService.UpdateBookingBySecretaryAsync(request));
        
        Assert.Equal("Secretary not found (Parameter 'SecretaryId')", exception.Message);
    }

    #endregion

    #region DeleteBookingAsync Tests

    [Fact]
    public async Task DeleteBookingAsync_ValidId_ReturnsTrue()
    {
        // Arrange
        var bookingId = 1;
        _mockBookingRepository.Setup(r => r.DeleteBookingAsync(bookingId)).ReturnsAsync(true);

        // Act
        var result = await _bookingService.DeleteBookingAsync(bookingId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteBookingAsync_InvalidId_ReturnsFalse()
    {
        // Arrange
        var bookingId = 999;
        _mockBookingRepository.Setup(r => r.DeleteBookingAsync(bookingId)).ReturnsAsync(false);

        // Act
        var result = await _bookingService.DeleteBookingAsync(bookingId);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region DeleteBookingBySecretaryAsync Tests

    [Fact]
    public async Task DeleteBookingBySecretaryAsync_ValidSecretary_DeletesBooking()
    {
        // Arrange
        var secretaryUser = new User
        {
            Id = 1,
            Role = UserRole.Secretary,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var bookingId = 1;
        var secretaryId = 1;

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(secretaryId)).ReturnsAsync(secretaryUser);
        _mockBookingRepository.Setup(r => r.DeleteBookingAsync(bookingId)).ReturnsAsync(true);

        // Act
        var result = await _bookingService.DeleteBookingBySecretaryAsync(bookingId, secretaryId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteBookingBySecretaryAsync_UserIsNotSecretary_ThrowsArgumentException()
    {
        // Arrange
        var nonSecretaryUser = new User
        {
            Id = 1,
            Role = UserRole.Employee,
            FirstName = "John",
            LastName = "Doe",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var bookingId = 1;
        var secretaryId = 1;

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(secretaryId)).ReturnsAsync(nonSecretaryUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _bookingService.DeleteBookingBySecretaryAsync(bookingId, secretaryId));
        
        Assert.Equal("Secretary not found (Parameter 'secretaryId')", exception.Message);
    }

    #endregion

    #region CheckinBookingAsync Tests

    [Fact]
    public async Task CheckinBookingAsync_ValidRequest_ReturnsTrue()
    {
        // Arrange
        var checkInRequest = new CheckInRequest
        {
            BookingId = 1,
            UserId = 1
        };

        var booking = new Booking
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.Today,
            Status = BookingStatus.Booked
        };

        _mockBookingRepository.Setup(r => r.GetBookingByIdAsync(1)).ReturnsAsync(booking);
        _mockBookingRepository.Setup(r => r.CheckinBookingAsync(checkInRequest)).ReturnsAsync(true);

        // Act
        var result = await _bookingService.CheckinBookingAsync(checkInRequest);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckinBookingAsync_NullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _bookingService.CheckinBookingAsync(null!));
        
        Assert.Equal("Check-in request cannot be null (Parameter 'checkInRequest')", exception.Message);
    }

    [Fact]
    public async Task CheckinBookingAsync_BookingNotFound_ReturnsFalse()
    {
        // Arrange
        var checkInRequest = new CheckInRequest
        {
            BookingId = 999,
            UserId = 1
        };

        _mockBookingRepository.Setup(r => r.GetBookingByIdAsync(999)).ReturnsAsync((Booking?)null);

        // Act
        var result = await _bookingService.CheckinBookingAsync(checkInRequest);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CheckinBookingAsync_BookingNotBooked_ReturnsFalse()
    {
        // Arrange
        var checkInRequest = new CheckInRequest
        {
            BookingId = 1,
            UserId = 1
        };

        var booking = new Booking
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.Today,
            Status = BookingStatus.Completed // Not Booked
        };

        _mockBookingRepository.Setup(r => r.GetBookingByIdAsync(1)).ReturnsAsync(booking);

        // Act
        var result = await _bookingService.CheckinBookingAsync(checkInRequest);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task CheckinBookingAsync_WrongUser_ReturnsFalse()
    {
        // Arrange
        var checkInRequest = new CheckInRequest
        {
            BookingId = 1,
            UserId = 2 // Different user
        };

        var booking = new Booking
        {
            Id = 1,
            UserId = 1, // Original user
            Date = DateTime.Today,
            Status = BookingStatus.Booked
        };

        _mockBookingRepository.Setup(r => r.GetBookingByIdAsync(1)).ReturnsAsync(booking);

        // Act
        var result = await _bookingService.CheckinBookingAsync(checkInRequest);

        // Assert
        Assert.False(result);
    }

    #endregion
}