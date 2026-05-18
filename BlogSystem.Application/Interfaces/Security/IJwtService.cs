using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Interfaces.Security;

public interface IJwtService
{
    string GenerateToken(User user);
}
