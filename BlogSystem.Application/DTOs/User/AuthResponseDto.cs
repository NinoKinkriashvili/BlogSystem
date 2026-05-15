using BlogSystem.Domain.Enums;

namespace BlogSystem.Application.DTOs.User;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole UserRole { get; set; }
    public string Token { get; set; } = string.Empty;
}
