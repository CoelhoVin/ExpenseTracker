using ExpenseTracker.Application.DTOs.Auth;
using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Domain;
using Moq;
using Xunit;

namespace ExpenseTracker.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_ShouldCreateUser_WhenEmailIsNotRegistered()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var passwordService = new Mock<IPasswordService>();
        var jwtService = new Mock<IJwtService>();

        userRepository
            .Setup(x => x.GetByEmailAsync("john@example.com"))
            .ReturnsAsync((User?)null);

        passwordService
            .Setup(x => x.HashPassword("Password123"))
            .Returns("hashed-password");

        var service = new AuthService(
            userRepository.Object,
            passwordService.Object,
            jwtService.Object);

        var request = new RegisterRequest
        {
            Name = "John",
            Email = "john@example.com",
            Password = "Password123"
        };

        // Act
        var result = await service.RegisterAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.Equal("John", result.Name);
        Assert.Equal("john@example.com", result.Email);

        userRepository.Verify(
            x => x.AddAsync(It.Is<User>(user =>
                user.Name == "John" &&
                user.Email == "john@example.com" &&
                user.PasswordHash == "hashed-password")),
            Times.Once);

        userRepository.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        passwordService.Verify(
            x => x.HashPassword("Password123"),
            Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenEmailIsAlreadyRegistered()
    {
        // Arrange
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Email = "john@example.com",
            PasswordHash = "hashed-password",
            CreatedAt = DateTime.UtcNow
        };

        var userRepository = new Mock<IUserRepository>();
        var passwordService = new Mock<IPasswordService>();
        var jwtService = new Mock<IJwtService>();

        userRepository
            .Setup(x => x.GetByEmailAsync("john@example.com"))
            .ReturnsAsync(existingUser);

        var service = new AuthService(
            userRepository.Object,
            passwordService.Object,
            jwtService.Object);

        var request = new RegisterRequest
        {
            Name = "Another John",
            Email = "john@example.com",
            Password = "Password123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.RegisterAsync(request));

        passwordService.Verify(
            x => x.HashPassword(It.IsAny<string>()),
            Times.Never);

        userRepository.Verify(
            x => x.AddAsync(It.IsAny<User>()),
            Times.Never);

        userRepository.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnAuthResponse_WhenCredentialsAreValid()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Name = "John",
            Email = "john@example.com",
            PasswordHash = "hashed-password",
            CreatedAt = DateTime.UtcNow
        };

        var userRepository = new Mock<IUserRepository>();
        var passwordService = new Mock<IPasswordService>();
        var jwtService = new Mock<IJwtService>();

        userRepository
            .Setup(x => x.GetByEmailAsync("john@example.com"))
            .ReturnsAsync(user);

        passwordService
            .Setup(x => x.VerifyPassword(
                "Password123",
                "hashed-password"))
            .Returns(true);

        jwtService
            .Setup(x => x.GenerateToken(
                userId,
                "John",
                "john@example.com"))
            .Returns("fake-jwt-token");

        var service = new AuthService(
            userRepository.Object,
            passwordService.Object,
            jwtService.Object);

        var request = new LoginRequest
        {
            Email = "john@example.com",
            Password = "Password123"
        };

        // Act
        var result = await service.LoginAsync(request);

        // Assert
        Assert.Equal("fake-jwt-token", result.Token);
        Assert.Equal(userId, result.UserId);
        Assert.Equal("John", result.Name);
        Assert.Equal("john@example.com", result.Email);

        passwordService.Verify(
            x => x.VerifyPassword(
                "Password123",
                "hashed-password"),
            Times.Once);

        jwtService.Verify(
            x => x.GenerateToken(
                userId,
                "John",
                "john@example.com"),
            Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordIsInvalid()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John",
            Email = "john@example.com",
            PasswordHash = "hashed-password",
            CreatedAt = DateTime.UtcNow
        };

        var userRepository = new Mock<IUserRepository>();
        var passwordService = new Mock<IPasswordService>();
        var jwtService = new Mock<IJwtService>();

        userRepository
            .Setup(x => x.GetByEmailAsync("john@example.com"))
            .ReturnsAsync(user);

        passwordService
            .Setup(x => x.VerifyPassword(
                "WrongPassword",
                "hashed-password"))
            .Returns(false);

        var service = new AuthService(
            userRepository.Object,
            passwordService.Object,
            jwtService.Object);

        var request = new LoginRequest
        {
            Email = "john@example.com",
            Password = "WrongPassword"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.LoginAsync(request));

        jwtService.Verify(
            x => x.GenerateToken(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);
    }
}