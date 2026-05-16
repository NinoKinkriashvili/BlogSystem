using AutoMapper;
using BlogSystem.Application.DTOs.Shared;
using BlogSystem.Application.DTOs.User;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;

    public UserService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        // TODO
        return null;
    }

    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken ct)
    {
        // TODO
        return null;
    }

    public async Task<UserDto?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        // TODO
        return null;
    }

    public async Task<PagedResultDto<UserDto>> GetAllAsync(int page, int itemPerPage, CancellationToken ct)
    {
        // TODO
        return new PagedResultDto<UserDto>
        {
            Items = new List<UserDto>(),
            TotalCount = 0,
            Page = page,
            PageSize = itemPerPage
        };
    }

    public async Task<UserDto> CreateAsync(RegisterUserDto dto, CancellationToken ct)
    {
        var user = _mapper.Map<User>(dto);

        // TODO: password hashing, repository save

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto, Guid userIdByWho, CancellationToken ct)
    {
        // TODO: get from repository
        var user = new User();

        _mapper.Map(dto, user);

        // TODO: update audit fields

        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteAsync(Guid id, Guid userIdByWho, CancellationToken ct)
    {
        // TODO
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        // TODO
        return false;
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct)
    {
        // TODO
        return false;
    }
}
