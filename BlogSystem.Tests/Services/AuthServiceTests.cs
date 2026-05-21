using Moq;
using BlogSystem.Application.Services;
using BlogSystem.Application.Interfaces.Repositories;
using BlogSystem.Application.Interfaces.Security;
using AutoMapper;
using BlogSystem.Application.DTOs.User;
using BlogSystem.Application.Exceptions;
using BlogSystem.Domain.Entities;

namespace BlogSystem.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IJwtService> _jwtServiceMock = new();

    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService(
            _mapperMock.Object,
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtServiceMock.Object
        );
    }

    private readonly RegisterUserDto _dto = new()
    {
        Email = "test@test.com",
        UserName = "test",
        Password = "123"
    };

    [Fact]
    public async Task Register_ShouldThrow_WhenUsernameAlreadyExists()
    {
        _userRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(_dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.ExistsByUsernameAsync(_dto.UserName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            _authService.RegisterAsync(_dto, CancellationToken.None));
    }

    [Fact]
    public async Task Register_ShouldThrow_WhenEmailAlreadyExists()
    {
        _userRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(_dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<BadRequestException>(() =>
            _authService.RegisterAsync(_dto, CancellationToken.None));
    }

    [Fact]
    public async Task Register_ShouldCreateUser_WhenDataIsValid()
    {
        _userRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mapperMock
            .Setup(x => x.Map<User>(_dto))
            .Returns(new User());

        _passwordHasherMock
            .Setup(x => x.Hash(_dto.Password))
            .Returns("hashed_password");

        _jwtServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns("token");

        await _authService.RegisterAsync(_dto, CancellationToken.None);

        _userRepositoryMock.Verify(x =>
                x.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
