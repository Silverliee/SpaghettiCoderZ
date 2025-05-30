using Moq;
using ReservationAPI.Models;
using ReservationAPI.Repositories;
using ReservationAPI.Services;
using Xunit;

namespace ReservationAPI.Tests.Services;

public class ParkingServiceTests
{
    private readonly Mock<IParkingRepository> _mockParkingRepository;
    private readonly ParkingService _parkingService;

    public ParkingServiceTests()
    {
        _mockParkingRepository = new Mock<IParkingRepository>();
        _parkingService = new ParkingService(_mockParkingRepository.Object);
    }

    #region GetAllParkingSlotsAsync Tests

    [Fact]
    public async Task GetAllParkingSlotsAsync_ReturnsAllParkingSlots()
    {
        // Arrange
        var expectedSlots = new List<ParkingSlot>
        {
            new() { Id = 1, Row = "A", Column = 1, HasCharger = true, InMaintenance = false },
            new() { Id = 2, Row = "A", Column = 2, HasCharger = false, InMaintenance = false },
            new() { Id = 3, Row = "B", Column = 1, HasCharger = true, InMaintenance = true }
        };

        _mockParkingRepository.Setup(r => r.GetParkingSlotsAsync())
            .ReturnsAsync(expectedSlots);

        // Act
        var result = await _parkingService.GetAllParkingSlotsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(expectedSlots, result);
        
        _mockParkingRepository.Verify(r => r.GetParkingSlotsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllParkingSlotsAsync_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        var emptySlots = new List<ParkingSlot>();
        _mockParkingRepository.Setup(r => r.GetParkingSlotsAsync())
            .ReturnsAsync(emptySlots);

        // Act
        var result = await _parkingService.GetAllParkingSlotsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region GetSlotByIdAsync Tests

    [Fact]
    public async Task GetSlotByIdAsync_ValidId_ReturnsParkingSlot()
    {
        // Arrange
        var slotId = 1;
        var expectedSlot = new ParkingSlot
        {
            Id = 1,
            Row = "A",
            Column = 1,
            HasCharger = true,
            InMaintenance = false
        };

        _mockParkingRepository.Setup(r => r.GetSlotByIdAsync(slotId))
            .ReturnsAsync(expectedSlot);

        // Act
        var result = await _parkingService.GetSlotByIdAsync(slotId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSlot.Id, result.Id);
        Assert.Equal(expectedSlot.Row, result.Row);
        Assert.Equal(expectedSlot.Column, result.Column);
        Assert.Equal(expectedSlot.HasCharger, result.HasCharger);
        Assert.Equal(expectedSlot.InMaintenance, result.InMaintenance);
        
        _mockParkingRepository.Verify(r => r.GetSlotByIdAsync(slotId), Times.Once);
    }

    [Fact]
    public async Task GetSlotByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        var slotId = 999;
        _mockParkingRepository.Setup(r => r.GetSlotByIdAsync(slotId))
            .ReturnsAsync((ParkingSlot?)null);

        // Act
        var result = await _parkingService.GetSlotByIdAsync(slotId);

        // Assert
        Assert.Null(result);
        _mockParkingRepository.Verify(r => r.GetSlotByIdAsync(slotId), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task GetSlotByIdAsync_NegativeOrZeroId_CallsRepository(int slotId)
    {
        // Arrange
        _mockParkingRepository.Setup(r => r.GetSlotByIdAsync(slotId))
            .ReturnsAsync((ParkingSlot?)null);

        // Act
        var result = await _parkingService.GetSlotByIdAsync(slotId);

        // Assert
        Assert.Null(result);
        _mockParkingRepository.Verify(r => r.GetSlotByIdAsync(slotId), Times.Once);
    }

    #endregion

    #region GetAvailableSlotsAsync Tests

    [Fact]
    public async Task GetAvailableSlotsAsync_ValidDate_ReturnsAvailableSlots()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var expectedSlots = new List<ParkingSlot>
        {
            new() { Id = 1, Row = "A", Column = 1, HasCharger = true, InMaintenance = false },
            new() { Id = 2, Row = "A", Column = 2, HasCharger = false, InMaintenance = false }
        };

        _mockParkingRepository.Setup(r => r.GetAvailableSlotsAsync(date))
            .ReturnsAsync(expectedSlots);

        // Act
        var result = await _parkingService.GetAvailableSlotsAsync(date);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(expectedSlots, result);
        
        _mockParkingRepository.Verify(r => r.GetAvailableSlotsAsync(date), Times.Once);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_NoAvailableSlots_ReturnsEmptyList()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var emptySlots = new List<ParkingSlot>();

        _mockParkingRepository.Setup(r => r.GetAvailableSlotsAsync(date))
            .ReturnsAsync(emptySlots);

        // Act
        var result = await _parkingService.GetAvailableSlotsAsync(date);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_FutureDate_ReturnsAvailableSlots()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));
        var expectedSlots = new List<ParkingSlot>
        {
            new() { Id = 1, Row = "A", Column = 1, HasCharger = true, InMaintenance = false }
        };

        _mockParkingRepository.Setup(r => r.GetAvailableSlotsAsync(futureDate))
            .ReturnsAsync(expectedSlots);

        // Act
        var result = await _parkingService.GetAvailableSlotsAsync(futureDate);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(expectedSlots, result);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_PastDate_ReturnsAvailableSlots()
    {
        // Arrange
        var pastDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-5));
        var expectedSlots = new List<ParkingSlot>
        {
            new() { Id = 1, Row = "A", Column = 1, HasCharger = true, InMaintenance = false }
        };

        _mockParkingRepository.Setup(r => r.GetAvailableSlotsAsync(pastDate))
            .ReturnsAsync(expectedSlots);

        // Act
        var result = await _parkingService.GetAvailableSlotsAsync(pastDate);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(expectedSlots, result);
    }

    #endregion

    #region AddParkingSlotAsync Tests

    [Fact]
    public async Task AddParkingSlotAsync_ValidParkingSlot_CallsRepository()
    {
        // Arrange
        var parkingSlot = new ParkingSlot
        {
            Row = "A",
            Column = 1,
            HasCharger = true,
            InMaintenance = false
        };

        // Act
        await _parkingService.AddParkingSlotAsync(parkingSlot);

        // Assert
        _mockParkingRepository.Verify(r => r.CreateParkingSlotAsync(parkingSlot), Times.Once);
    }

    [Fact]
    public async Task AddParkingSlotAsync_NullParkingSlot_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _parkingService.AddParkingSlotAsync(null!));
        
        Assert.Equal("Parking slot cannot be null. (Parameter 'parkingSlot')", exception.Message);
        
        _mockParkingRepository.Verify(r => r.CreateParkingSlotAsync(It.IsAny<ParkingSlot>()), Times.Never);
    }

    [Fact]
    public async Task AddParkingSlotAsync_ValidSlotWithCharger_CallsRepository()
    {
        // Arrange
        var parkingSlot = new ParkingSlot
        {
            Row = "B",
            Column = 5,
            HasCharger = true,
            InMaintenance = false
        };

        // Act
        await _parkingService.AddParkingSlotAsync(parkingSlot);

        // Assert
        _mockParkingRepository.Verify(r => r.CreateParkingSlotAsync(parkingSlot), Times.Once);
    }

    [Fact]
    public async Task AddParkingSlotAsync_SlotInMaintenance_CallsRepository()
    {
        // Arrange
        var parkingSlot = new ParkingSlot
        {
            Row = "C",
            Column = 3,
            HasCharger = false,
            InMaintenance = true
        };

        // Act
        await _parkingService.AddParkingSlotAsync(parkingSlot);

        // Assert
        _mockParkingRepository.Verify(r => r.CreateParkingSlotAsync(parkingSlot), Times.Once);
    }

    #endregion

    #region UpdateParkingSlotAsync Tests

    [Fact]
    public async Task UpdateParkingSlotAsync_ValidParkingSlot_CallsRepository()
    {
        // Arrange
        var parkingSlot = new ParkingSlot
        {
            Id = 1,
            Row = "A",
            Column = 1,
            HasCharger = true,
            InMaintenance = false
        };

        // Act
        await _parkingService.UpdateParkingSlotAsync(parkingSlot);

        // Assert
        _mockParkingRepository.Verify(r => r.UpdateParkingSlotAsync(parkingSlot), Times.Once);
    }

    [Fact]
    public async Task UpdateParkingSlotAsync_NullParkingSlot_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _parkingService.UpdateParkingSlotAsync(null!));
        
        Assert.Equal("Parking slot cannot be null. (Parameter 'parkingSlot')", exception.Message);
        
        _mockParkingRepository.Verify(r => r.UpdateParkingSlotAsync(It.IsAny<ParkingSlot>()), Times.Never);
    }

    [Fact]
    public async Task UpdateParkingSlotAsync_UpdateMaintenanceStatus_CallsRepository()
    {
        // Arrange
        var parkingSlot = new ParkingSlot
        {
            Id = 1,
            Row = "A",
            Column = 1,
            HasCharger = true,
            InMaintenance = true // Changed to maintenance
        };

        // Act
        await _parkingService.UpdateParkingSlotAsync(parkingSlot);

        // Assert
        _mockParkingRepository.Verify(r => r.UpdateParkingSlotAsync(parkingSlot), Times.Once);
    }

    [Fact]
    public async Task UpdateParkingSlotAsync_UpdateChargerStatus_CallsRepository()
    {
        // Arrange
        var parkingSlot = new ParkingSlot
        {
            Id = 1,
            Row = "A",
            Column = 1,
            HasCharger = false, // Changed charger status
            InMaintenance = false
        };

        // Act
        await _parkingService.UpdateParkingSlotAsync(parkingSlot);

        // Assert
        _mockParkingRepository.Verify(r => r.UpdateParkingSlotAsync(parkingSlot), Times.Once);
    }

    #endregion

    #region DeleteParkingSlotAsync Tests

    [Fact]
    public async Task DeleteParkingSlotAsync_ValidId_CallsRepository()
    {
        // Arrange
        var slotId = 1;

        // Act
        await _parkingService.DeleteParkingSlotAsync(slotId);

        // Assert
        _mockParkingRepository.Verify(r => r.DeleteParkingSlotAsync(slotId), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public async Task DeleteParkingSlotAsync_InvalidId_ThrowsArgumentException(int invalidId)
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _parkingService.DeleteParkingSlotAsync(invalidId));
        
        Assert.Equal("Invalid parking slot ID. (Parameter 'id')", exception.Message);
        
        _mockParkingRepository.Verify(r => r.DeleteParkingSlotAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteParkingSlotAsync_LargeValidId_CallsRepository()
    {
        // Arrange
        var slotId = 999999;

        // Act
        await _parkingService.DeleteParkingSlotAsync(slotId);

        // Assert
        _mockParkingRepository.Verify(r => r.DeleteParkingSlotAsync(slotId), Times.Once);
    }

    #endregion

    #region Repository Exception Handling Tests

    [Fact]
    public async Task GetAllParkingSlotsAsync_RepositoryThrowsException_ThrowsException()
    {
        // Arrange
        _mockParkingRepository.Setup(r => r.GetParkingSlotsAsync())
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _parkingService.GetAllParkingSlotsAsync());
        
        Assert.Equal("Database connection failed", exception.Message);
    }

    [Fact]
    public async Task GetSlotByIdAsync_RepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var slotId = 1;
        _mockParkingRepository.Setup(r => r.GetSlotByIdAsync(slotId))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _parkingService.GetSlotByIdAsync(slotId));
        
        Assert.Equal("Database error", exception.Message);
    }

    [Fact]
    public async Task GetAvailableSlotsAsync_RepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        _mockParkingRepository.Setup(r => r.GetAvailableSlotsAsync(date))
            .ThrowsAsync(new Exception("Database timeout"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _parkingService.GetAvailableSlotsAsync(date));
        
        Assert.Equal("Database timeout", exception.Message);
    }

    [Fact]
    public async Task AddParkingSlotAsync_RepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var parkingSlot = new ParkingSlot
        {
            Row = "A",
            Column = 1,
            HasCharger = true,
            InMaintenance = false
        };

        _mockParkingRepository.Setup(r => r.CreateParkingSlotAsync(parkingSlot))
            .ThrowsAsync(new Exception("Duplicate slot"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _parkingService.AddParkingSlotAsync(parkingSlot));
        
        Assert.Equal("Duplicate slot", exception.Message);
    }

    [Fact]
    public async Task UpdateParkingSlotAsync_RepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var parkingSlot = new ParkingSlot
        {
            Id = 1,
            Row = "A",
            Column = 1,
            HasCharger = true,
            InMaintenance = false
        };

        _mockParkingRepository.Setup(r => r.UpdateParkingSlotAsync(parkingSlot))
            .ThrowsAsync(new Exception("Slot not found"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _parkingService.UpdateParkingSlotAsync(parkingSlot));
        
        Assert.Equal("Slot not found", exception.Message);
    }

    [Fact]
    public async Task DeleteParkingSlotAsync_RepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var slotId = 1;
        _mockParkingRepository.Setup(r => r.DeleteParkingSlotAsync(slotId))
            .ThrowsAsync(new Exception("Cannot delete slot in use"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _parkingService.DeleteParkingSlotAsync(slotId));
        
        Assert.Equal("Cannot delete slot in use", exception.Message);
    }

    #endregion
}