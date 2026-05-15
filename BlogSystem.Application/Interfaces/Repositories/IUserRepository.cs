using BlogSystem.Domain.Entities;

namespace BlogSystem.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByUserNameAsync(string username);

    Task<IEnumerable<User>> GetAllAsync(int page, int itemPerPage);

    Task<int> GetCountAsync();

    Task CreateAsync(User user);

    Task UpdateAsync(User user);

    Task DeleteAsync(User user);

    Task<bool> ExistsByEmailAsync(string email);

    Task<bool> ExistsByUsernameAsync(string username);
}
