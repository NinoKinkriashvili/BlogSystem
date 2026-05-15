using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<User?> GetByEmailAsync(string email, CancellationToken ct);

    Task<User?> GetByUserNameAsync(string username, CancellationToken ct);

    Task<IEnumerable<User>> GetAllAsync(int page, int itemPerPage, CancellationToken ct);

    Task<int> GetCountAsync(CancellationToken ct = default);

    Task CreateAsync(User user, CancellationToken ct);

    Task UpdateAsync(User user, CancellationToken ct);

    Task DeleteAsync(User user, CancellationToken ct);

    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);

    Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct);
}
