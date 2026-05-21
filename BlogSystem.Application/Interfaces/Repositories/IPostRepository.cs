using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Interfaces.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<Post?> GetByIdForAdminAsync(Guid id, CancellationToken ct);

    Task<IEnumerable<Post>> GetAllAsync(int page, int itemPerPage, CancellationToken ct);

    Task<IEnumerable<Post>> GetAllForAdminAsync(int page, int itemPerPage, CancellationToken ct);

    Task<IEnumerable<Post>> SearchAsync(string? searchResult, int page, int itemPerPage, CancellationToken ct);

    Task<int> GetCountAsync(string? searchResult = null, CancellationToken ct = default);

    Task<List<Post>> GetByUserIdAsync(Guid userId, string? search, int page, int itemPerPage, CancellationToken ct);
    Task<int> GetCountByUserIdAsync(Guid userId, string? search, CancellationToken ct);

    Task<IEnumerable<Post>> GetByUserIdForAdminAsync(Guid userId, int page, int itemPerPage, CancellationToken ct);

    Task CreateAsync(Post post, CancellationToken ct);

    Task UpdateAsync(Post post, CancellationToken ct);

    Task DeleteAsync(Post post, CancellationToken ct);

    Task<int> GetCountByUserIdAsync(Guid userId, CancellationToken ct);
}
