using AutoMapper;
using BlogSystem.Application.DTOs.User;
using BlogSystem.Application.Exceptions;
using BlogSystem.Application.Interfaces.Repositories;
using BlogSystem.Application.Interfaces.Security;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(
        IMapper mapper,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email, ct);

        if (user == null)
            throw new UnauthorizedException("Invalid email or password");

        var isValid = _passwordHasher.Verify(dto.Password, user.PasswordHash);

        if (!isValid)
            throw new UnauthorizedException("Invalid email or password");

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            UserRole = user.Role,
            Token = token
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto, CancellationToken ct)
    {
        var emailExists = await _userRepository.ExistsByEmailAsync(dto.Email, ct);
        if (emailExists)
            throw new BadRequestException("Email already exists");

        var usernameExists = await _userRepository.ExistsByUsernameAsync(dto.UserName, ct);
        if (usernameExists)
            throw new BadRequestException("Username already exists");

        var user = _mapper.Map<User>(dto);

        user.PasswordHash = _passwordHasher.Hash(dto.Password);
        user.CreatedAt = DateTime.UtcNow;

        await _userRepository.CreateAsync(user, ct);

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            UserRole = user.Role,
            Token = token
        };
    }
}
