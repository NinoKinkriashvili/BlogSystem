using AutoMapper;
using BlogSystem.Application.DTOs.Post;
using BlogSystem.Application.DTOs.Shared;
using BlogSystem.Application.Exceptions;
using BlogSystem.Application.Interfaces.Repositories;
using BlogSystem.Application.Interfaces.Security;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Services;

public class PostService : IPostService
{
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public PostService(
        IMapper mapper,
        IPostRepository postRepository,
        IUserRepository userRepository,
        ICurrentUser currentUser)
    {
        _mapper = mapper;
        _postRepository = postRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<PostDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var post = await _postRepository.GetByIdAsync(id, ct);
        return post == null ? null : _mapper.Map<PostDto>(post);
    }

    public async Task<PagedResultDto<PostDto>> GetAllAsync(int page, int itemPerPage, CancellationToken ct)
    {
        var posts = await _postRepository.GetAllAsync(page, itemPerPage, ct);
        var count = await _postRepository.GetCountAsync(null, ct);

        return new PagedResultDto<PostDto>
        {
            Items = _mapper.Map<List<PostDto>>(posts),
            TotalCount = count,
            Page = page,
            PageSize = itemPerPage
        };
    }

    public async Task<PagedResultDto<PostDto>> SearchAsync(string? searchResult, int page, int itemPerPage, CancellationToken ct)
    {
        var posts = await _postRepository.SearchAsync(searchResult, page, itemPerPage, ct);
        var count = await _postRepository.GetCountAsync(searchResult, ct);

        return new PagedResultDto<PostDto>
        {
            Items = _mapper.Map<List<PostDto>>(posts),
            TotalCount = count,
            Page = page,
            PageSize = itemPerPage
        };
    }

    public async Task<PagedResultDto<PostDto>> GetByUserIdAsync(Guid userId, int page, int itemPerPage, CancellationToken ct)
    {
        return await GetByUserIdAsync(userId, null, page, itemPerPage, ct);
    }

    public async Task<PagedResultDto<PostDto>> GetByUserIdAsync(Guid userId, string? search, int page, int itemPerPage, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(userId, ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var posts = await _postRepository.GetByUserIdAsync(userId, search, page, itemPerPage, ct);
        var count = await _postRepository.GetCountByUserIdAsync(userId, search, ct);

        return new PagedResultDto<PostDto>
        {
            Items = _mapper.Map<List<PostDto>>(posts),
            TotalCount = count,
            Page = page,
            PageSize = itemPerPage
        };
    }

    public async Task<PostDto> CreateAsync(CreatePostDto dto, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        if (!_currentUser.IsAuthenticated || userId == null)
            throw new UnauthorizedException("User not authenticated");

        var user = await _userRepository.GetByIdAsync(userId.Value, ct);
        if (user == null)
            throw new NotFoundException("User not found");

        var post = _mapper.Map<Post>(dto);

        post.UserId = userId.Value;
        post.CreatedAt = DateTime.UtcNow;

        await _postRepository.CreateAsync(post, ct);

        return _mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> UpdateAsync(Guid id, UpdatePostDto dto, CancellationToken ct)
    {
        var post = await _postRepository.GetByIdAsync(id, ct);

        if (post == null)
            throw new NotFoundException("Post not found");

        if (post.UserId != _currentUser.UserId && !_currentUser.IsAdmin)
            throw new ForbiddenException("You are not allowed to update this post");

        _mapper.Map(dto, post);

        post.UpdatedAt = DateTime.UtcNow;

        await _postRepository.UpdateAsync(post, ct);

        return _mapper.Map<PostDto>(post);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var post = await _postRepository.GetByIdAsync(id, ct);

        if (post == null)
            throw new NotFoundException("Post not found");

        if (post.UserId != _currentUser.UserId && !_currentUser.IsAdmin)
            throw new ForbiddenException("You are not allowed to delete this post");

        await _postRepository.DeleteAsync(post, ct);
    }
}
