using BlogSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure.Persistence;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {

    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);

        modelBuilder.Entity<Post>()
            .HasQueryFilter(x => !x.IsDeleted);
    }
}
