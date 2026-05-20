using BlogSystem.Application.DTOs.Post;
using BlogSystem.Application.DTOs.Shared;

namespace BlogSystem.Application.Interfaces.Services;

public interface IPostService
{
    Task<PostDto?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<PagedResultDto<PostDto>> GetAllAsync(int page, int itemPerPage, CancellationToken ct);

    Task<PagedResultDto<PostDto>> SearchAsync(string? searchResult, int page, int itemPerPage, CancellationToken ct);

    Task<PagedResultDto<PostDto>> GetByUserIdAsync(Guid userId, int page, int itemPerPage, CancellationToken ct);

    Task<PostDto> CreateAsync(CreatePostDto dto, CancellationToken ct);

    Task<PostDto> UpdateAsync(Guid id, UpdatePostDto dto, CancellationToken ct);

    Task DeleteAsync(Guid id, CancellationToken ct);
}
