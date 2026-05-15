using BlogSystem.Application.DTOs.Shared;
using BlogSystem.Application.DTOs.User;

namespace BlogSystem.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<UserDto?> GetByEmailAsync(string email, CancellationToken ct);

    Task<UserDto?> GetByUsernameAsync(string username, CancellationToken ct);

    Task<PagedResultDto<UserDto>> GetAllAsync(int page, int itemPerPage, CancellationToken ct);

    Task<UserDto> CreateAsync(RegisterUserDto dto, CancellationToken ct);

    Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto, Guid userIdByWho, CancellationToken ct);

    Task DeleteAsync(Guid id, Guid userIdByWho, CancellationToken ct);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);

    Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct);
}
