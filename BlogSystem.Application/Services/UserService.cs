using AutoMapper;
using BlogSystem.Application.DTOs.Shared;
using BlogSystem.Application.DTOs.User;
using BlogSystem.Application.Exceptions;
using BlogSystem.Application.Interfaces.Repositories;
using BlogSystem.Application.Interfaces.Security;
using BlogSystem.Application.Interfaces.Services;
using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IMapper mapper,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(id, ct);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(email, ct);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        var user = await _userRepository.GetByUserNameAsync(username, ct);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<PagedResultDto<UserDto>> GetAllAsync(int page, int itemPerPage, CancellationToken ct)
    {
        var users = await _userRepository.GetAllAsync(page, itemPerPage, ct);
        var count = await _userRepository.GetCountAsync(ct);

        return new PagedResultDto<UserDto>
        {
            Items = _mapper.Map<List<UserDto>>(users),
            TotalCount = count,
            Page = page,
            PageSize = itemPerPage
        };
    }

    public async Task<UserDto> CreateAsync(RegisterUserDto dto, CancellationToken ct)
    {
        var emailExists = await _userRepository.ExistsByEmailAsync(dto.Email, ct);
        if (emailExists)
            throw new BadRequestException("Email already exists");

        var usernameExists = await _userRepository.ExistsByUsernameAsync(dto.UserName, ct);
        if (usernameExists)
            throw new BadRequestException("Username already exists");

        var user = _mapper.Map<User>(dto);

        user.PasswordHash = _passwordHasher.Hash(dto.Password);
        user.CreatedAt = DateTime.UtcNow;

        await _userRepository.CreateAsync(user, ct);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto, Guid userIdByWho, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(id, ct);

        if (user == null)
            throw new NotFoundException("User not found");

        if (user.Id != userIdByWho)
            throw new ForbiddenException("You are not allowed to update this user");

        _mapper.Map(dto, user);

        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user, ct);

        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteAsync(Guid id, Guid userIdByWho, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(id, ct);

        if (user == null)
            throw new NotFoundException("User not found");

        if (user.Id != userIdByWho)
            throw new ForbiddenException("You are not allowed to delete this user");

        await _userRepository.DeleteAsync(user, ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        return await _userRepository.ExistsByEmailAsync(email, ct);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct)
    {
        return await _userRepository.ExistsByUsernameAsync(username, ct);
    }
}
