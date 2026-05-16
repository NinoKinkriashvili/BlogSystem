using AutoMapper;
using BlogSystem.Application.DTOs.User;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;

    public AuthService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct)
    {
        // TODO
        var user = new User();

        // TODO
        var token = "fake-jwt-token";

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
        var user = _mapper.Map<User>(dto);

        // TODO
        user.PasswordHash = dto.Password;

        // TODO
        var token = "fake-jwt-token";

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
