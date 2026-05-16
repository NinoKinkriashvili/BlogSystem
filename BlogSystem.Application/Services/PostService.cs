using AutoMapper;
using BlogSystem.Application.DTOs.Post;
using BlogSystem.Application.DTOs.Shared;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Services;

public class PostService : IPostService
{
    private readonly IMapper _mapper;

    public PostService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<PostDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        // TODO
        return null;
    }

    public async Task<PagedResultDto<PostDto>> GetAllAsync(int page, int itemPerPage, CancellationToken ct)
    {
        // TODO
        return new PagedResultDto<PostDto>
        {
            Items = new List<PostDto>(),
            TotalCount = 0,
            Page = page,
            PageSize = itemPerPage
        };
    }

    public async Task<PagedResultDto<PostDto>> SearchAsync(string? searchResult, int page, int itemPerPage, CancellationToken ct)
    {
        // TODO
        return new PagedResultDto<PostDto>
        {
            Items = new List<PostDto>(),
            TotalCount = 0,
            Page = page,
            PageSize = itemPerPage
        };
    }

    public async Task<PagedResultDto<PostDto>> GetByUserIdAsync(Guid userId, int page, int itemPerPage, CancellationToken ct)
    {
        // TODO
        return new PagedResultDto<PostDto>
        {
            Items = new List<PostDto>(),
            TotalCount = 0,
            Page = page,
            PageSize = itemPerPage
        };
    }

    public async Task<PostDto> CreateAsync(CreatePostDto dto, Guid userId, CancellationToken ct)
    {
        var post = _mapper.Map<Post>(dto);

        post.UserId = userId;
        post.CreatedAt = DateTime.UtcNow;

        // TODO
        return _mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> UpdateAsync(Guid id, UpdatePostDto dto, Guid userId, CancellationToken ct)
    {
        // TODO: get from repository later
        var post = new Post();

        _mapper.Map(dto, post);

        post.UpdatedAt = DateTime.UtcNow;
        // TODO
        return _mapper.Map<PostDto>(post);
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken ct)
    {
        // TODO
    }
}
