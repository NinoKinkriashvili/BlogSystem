using BlogSystem.Application.DTOs.User;

namespace BlogSystem.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken ct);

    Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto, CancellationToken ct);
}
