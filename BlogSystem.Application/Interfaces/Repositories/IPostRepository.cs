using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Interfaces.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id);

    Task<IEnumerable<Post>> GetAllAsync(int page, int itemPerPage);

    Task<IEnumerable<Post>> SearchAsync(string? searchResult, int page, int itemPerPage);

    Task<int> GetCountAsync(string? searchResult = null);

    Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId, int page, int itemPerPage);

    Task CreateAsync(Post post);

    Task UpdateAsync(Post post);

    Task DeleteAsync(Post post);
}
