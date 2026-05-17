using BlogSystem.Application.Interfaces.Repositories;
using BlogSystem.Domain.Entities;
using BlogSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BlogDbContext _context;

    public UserRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email, ct);
    }

    public async Task<User?> GetByUserNameAsync(string username, CancellationToken ct)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.UserName == username, ct);
    }

    public async Task<IEnumerable<User>> GetAllAsync(int page, int itemPerPage, CancellationToken ct)
    {
        return await _context.Users
            .Skip((page - 1) * itemPerPage)
            .Take(itemPerPage)
            .ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(CancellationToken ct = default)
    {
        return await _context.Users
            .CountAsync(ct);
    }

    public async Task CreateAsync(User user, CancellationToken ct)
    {
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(User user, CancellationToken ct)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email, ct);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken ct)
    {
        return await _context.Users
            .AnyAsync(x => x.UserName == username, ct);
    }
}
