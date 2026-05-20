using BlogSystem.Application.Interfaces.Repositories;
using BlogSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BlogSystem.Infrastructure.Persistence;

namespace BlogSystem.Infrastructure.Persistence.Repositories;

public class PostRepository : IPostRepository
{
    private readonly BlogDbContext _context;

    public PostRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Posts
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IEnumerable<Post>> GetAllAsync(int page, int itemPerPage, CancellationToken ct)
    {
        var safePage = SafePage(page);

        return await _context.Posts
            .Include(x => x.User)
            .Skip((safePage - 1) * itemPerPage)
            .Take(itemPerPage)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Post>> SearchAsync(string? searchResult, int page, int itemPerPage, CancellationToken ct)
    {
        var query = _context.Posts
            .Include(x => x.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchResult))
        {
            query = query.Where(x =>
                x.Title.Contains(searchResult) ||
                x.Content.Contains(searchResult));
        }

        return await query
            .Skip((SafePage(page) - 1) * itemPerPage)
            .Take(itemPerPage)
            .ToListAsync(ct);
    }

    // Total number of posts
    public async Task<int> GetCountAsync(string? searchResult = null, CancellationToken ct = default)
    {
        var query = _context.Posts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchResult))
        {
            query = query.Where(x =>
                x.Title.Contains(searchResult) ||
                x.Content.Contains(searchResult));
        }

        return await query.CountAsync(ct);
    }

    public async Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId, int page, int itemPerPage, CancellationToken ct)
    {
        return await _context.Posts
            .Where(x => x.UserId == userId)
            .Include(x => x.User)
            .Skip((SafePage(page) - 1) * itemPerPage)
            .Take(itemPerPage)
            .ToListAsync(ct);
    }

    public async Task CreateAsync(Post post, CancellationToken ct)
    {
        await _context.Posts.AddAsync(post, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Post post, CancellationToken ct)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Post post, CancellationToken ct)
    {
        post.IsDeleted = true;
        post.UpdatedAt = DateTime.UtcNow;

        _context.Posts.Update(post);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<int> GetCountByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await _context.Posts
            .Where(x => x.UserId == userId)
            .CountAsync(ct);
    }

    private int SafePage(int page)
    {
        return page < 1 ? 1 : page;
    }
}
