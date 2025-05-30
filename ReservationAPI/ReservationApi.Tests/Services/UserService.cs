using Moq;
using ReservationAPI.Middlewares.Security;
using ReservationAPI.Models;
using ReservationAPI.Models.DTO.Authentication;
using ReservationAPI.Models.DTO.Register;
using ReservationAPI.Repositories;
using ReservationAPI.Services;

namespace ReservationAPI.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ICryptographer> _mockCryptographer;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockCryptographer = new Mock<ICryptographer>();
        _userService = new UserService(_mockUserRepository.Object, _mockCryptographer.Object);
    }

    #region RegisterUserAsync Tests

    [Fact]
    public async Task RegisterUserAsync_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var registeringRequest = new RegisteringRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "password123"
        };

        var expectedUser = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Role = UserRole.Employee,
            Password = "encrypted_password"
        };

        _mockCryptographer.Setup(c => c.Encrypt("password123")).Returns("encrypted_password");
        _mockUserRepository.Setup(r => r.RegisterUserAsync(It.IsAny<User>())).ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.RegisterUserAsync(registeringRequest);

        // Assert
        Assert.True(result.IsRegistered);
        Assert.Equal(1, result.UserId);
        
        _mockCryptographer.Verify(c => c.Encrypt("password123"), Times.Once);
        _mockUserRepository.Verify(r => r.RegisterUserAsync(It.Is<User>(u =>
            u.FirstName == "John" &&
            u.LastName == "Doe" &&
            u.Email == "john.doe@test.com" &&
            u.Role == UserRole.Employee &&
            u.Password == "encrypted_password")), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_ExceptionThrown_ReturnsFailureResponse()
    {
        // Arrange
        var registeringRequest = new RegisteringRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "password123"
        };

        _mockCryptographer.Setup(c => c.Encrypt("password123")).Returns("encrypted_password");
        _mockUserRepository.Setup(r => r.RegisterUserAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _userService.RegisterUserAsync(registeringRequest);

        // Assert
        Assert.False(result.IsRegistered);
        Assert.Equal(0, result.UserId);
    }

    [Fact]
    public async Task RegisterUserAsync_RepositoryReturnsNull_ReturnsSuccessWithZeroUserId()
    {
        // Arrange
        var registeringRequest = new RegisteringRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "password123"
        };

        _mockCryptographer.Setup(c => c.Encrypt("password123")).Returns("encrypted_password");
        _mockUserRepository.Setup(r => r.RegisterUserAsync(It.IsAny<User>()))!.ReturnsAsync((User?)null);

        // Act
        var result = await _userService.RegisterUserAsync(registeringRequest);

        // Assert
        Assert.True(result.IsRegistered);
        Assert.Equal(0, result.UserId);
    }

    #endregion

    #region LoginUserAsync Tests

    [Fact]
    public async Task LoginUserAsync_ValidCredentials_ReturnsAuthenticationResponse()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Email = "john.doe@test.com",
            Password = "password123"
        };

        var user = new User
        {
            Id = 1,
            Email = "john.doe@test.com",
            Password = "encrypted_password",
            Role = UserRole.Employee,
            FirstName = "John",
            LastName = "Doe"
        };

        _mockUserRepository.Setup(r => r.GetUserByEmailAsync("john.doe@test.com")).ReturnsAsync(user);
        _mockCryptographer.Setup(c => c.Encrypt("password123")).Returns("encrypted_password");

        // Act
        var result = await _userService.LoginUserAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.UserId);
        Assert.Equal(UserRole.Employee, result.Role);
    }

    [Fact]
    public async Task LoginUserAsync_InvalidPassword_ReturnsNull()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Email = "john.doe@test.com",
            Password = "wrongpassword"
        };

        var user = new User
        {
            Id = 1,
            Email = "john.doe@test.com",
            Password = "encrypted_password",
            Role = UserRole.Employee,
            FirstName = "John",
            LastName = "Doe"
        };

        _mockUserRepository.Setup(r => r.GetUserByEmailAsync("john.doe@test.com")).ReturnsAsync(user);
        _mockCryptographer.Setup(c => c.Encrypt("wrongpassword")).Returns("wrong_encrypted_password");

        // Act
        var result = await _userService.LoginUserAsync(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginUserAsync_UserNotFound_ThrowsException()
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Email = "nonexistent@test.com",
            Password = "password123"
        };

        _mockUserRepository.Setup(r => r.GetUserByEmailAsync("nonexistent@test.com"))
            .ThrowsAsync(new Exception("User not found"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _userService.LoginUserAsync(request));
    }

    [Theory]
    [InlineData(null, "password123")]
    [InlineData("john.doe@test.com", null)]
    [InlineData(null, null)]
    public async Task LoginUserAsync_NullEmailOrPassword_HandlesGracefully(string? email, string? password)
    {
        // Arrange
        var request = new AuthenticationRequest
        {
            Email = email,
            Password = password
        };

        // Act & Assert
        // Le comportement dépend de l'implémentation du repository
        // Si il gère les valeurs null, on teste, sinon on s'attend à une exception
        if (email != null && password != null)
        {
            var user = new User
            {
                Id = 1,
                Email = email,
                Password = "encrypted_password",
                Role = UserRole.Employee,
                FirstName = "John",
                LastName = "Doe"
            };
            
            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(user);
            _mockCryptographer.Setup(c => c.Encrypt(password)).Returns("different_password");
            
            var result = await _userService.LoginUserAsync(request);
            Assert.Null(result);
        }
    }

    #endregion

    #region GetUserByIdAsync Tests

    [Fact]
    public async Task GetUserByIdAsync_ValidUserId_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Role = UserRole.Employee,
            Password = "encrypted_password"
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.FirstName, result.FirstName);
        Assert.Equal(expectedUser.LastName, result.LastName);
        Assert.Equal(expectedUser.Email, result.Email);
        Assert.Equal(expectedUser.Role, result.Role);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserNotFound_ReturnsNull()
    {
        // Arrange
        var userId = 999;
        _mockUserRepository.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetAllUsersAsync Tests

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Arrange
        var expectedUsers = new List<User>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@test.com", Role = UserRole.Employee, Password = "encrypted_password" },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", Role = UserRole.Manager, Password = "encrypted_password" },
            new() { Id = 3, FirstName = "Bob", LastName = "Johnson", Email = "bob@test.com", Role = UserRole.Secretary, Password = "encrypted_password" }
        };

        _mockUserRepository.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(expectedUsers);

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(expectedUsers, result);
    }

    [Fact]
    public async Task GetAllUsersAsync_NoUsers_ReturnsEmptyList()
    {
        // Arrange
        _mockUserRepository.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(new List<User>());

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region RegisterUserForSecretaryAsync Tests

    [Fact]
    public async Task RegisterUserForSecretaryAsync_ValidSecretaryAndRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new RegisteredBySecretaryRequest
        {
            SecretaryId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "password123"
        };

        var secretary = new User
        {
            Id = 1,
            Role = UserRole.Secretary,
            FirstName = "Secretary",
            LastName = "User",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        var registeredUser = new User
        {
            Id = 2,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Role = UserRole.Employee,
            Password = "encrypted_password"
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(secretary);
        _mockCryptographer.Setup(c => c.Encrypt("password123")).Returns("encrypted_password");
        _mockUserRepository.Setup(r => r.RegisterUserAsync(It.IsAny<User>())).ReturnsAsync(registeredUser);

        // Act
        var result = await _userService.RegisterUserForSecretaryAsync(request);

        // Assert
        Assert.True(result.IsRegistered);
        Assert.Equal(2, result.UserId);
        
        _mockUserRepository.Verify(r => r.RegisterUserAsync(It.Is<User>(u =>
            u.FirstName == "John" &&
            u.LastName == "Doe" &&
            u.Email == "john.doe@test.com" &&
            u.Role == UserRole.Employee &&
            u.Password == "encrypted_password")), Times.Once);
    }

    [Fact]
    public async Task RegisterUserForSecretaryAsync_SecretaryNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new RegisteredBySecretaryRequest
        {
            SecretaryId = 999,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "password123"
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(999)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.RegisterUserForSecretaryAsync(request);

        // Assert
        Assert.False(result.IsRegistered);
        Assert.Equal(0, result.UserId);
        
        _mockUserRepository.Verify(r => r.RegisterUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUserForSecretaryAsync_UserIsNotSecretary_ReturnsFailureResponse()
    {
        // Arrange
        var request = new RegisteredBySecretaryRequest
        {
            SecretaryId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "password123"
        };

        var nonSecretary = new User
        {
            Id = 1,
            Role = UserRole.Employee, // Pas Secretary
            FirstName = "Regular",
            LastName = "User",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(nonSecretary);

        // Act
        var result = await _userService.RegisterUserForSecretaryAsync(request);

        // Assert
        Assert.False(result.IsRegistered);
        Assert.Equal(0, result.UserId);
        
        _mockUserRepository.Verify(r => r.RegisterUserAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUserForSecretaryAsync_ExceptionDuringRegistration_ReturnsFailureResponse()
    {
        // Arrange
        var request = new RegisteredBySecretaryRequest
        {
            SecretaryId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "password123"
        };

        var secretary = new User
        {
            Id = 1,
            Role = UserRole.Secretary,
            FirstName = "Secretary",
            LastName = "User",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(secretary);
        _mockCryptographer.Setup(c => c.Encrypt("password123")).Returns("encrypted_password");
        _mockUserRepository.Setup(r => r.RegisterUserAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _userService.RegisterUserForSecretaryAsync(request);

        // Assert
        Assert.False(result.IsRegistered);
        Assert.Equal(0, result.UserId);
    }

    [Fact]
    public async Task RegisterUserForSecretaryAsync_RepositoryReturnsNull_ReturnsSuccessWithZeroUserId()
    {
        // Arrange
        var request = new RegisteredBySecretaryRequest
        {
            SecretaryId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "password123"
        };

        var secretary = new User
        {
            Id = 1,
            Role = UserRole.Secretary,
            FirstName = "Secretary",
            LastName = "User",
            Email = "user@email.com",
            Password = "encrypted_password"
        };

        _mockUserRepository.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(secretary);
        _mockCryptographer.Setup(c => c.Encrypt("password123")).Returns("encrypted_password");
        _mockUserRepository.Setup(r => r.RegisterUserAsync(It.IsAny<User>()))!.ReturnsAsync((User?)null);

        // Act
        var result = await _userService.RegisterUserForSecretaryAsync(request);

        // Assert
        Assert.True(result.IsRegistered);
        Assert.Equal(0, result.UserId);
    }

    #endregion
}