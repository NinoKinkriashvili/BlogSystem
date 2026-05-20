using BlogSystem.Domain.Entities;
using BlogSystem.Domain.Enums;
using BlogSystem.Infrastructure.Persistence;
using BlogSystem.Application.Interfaces.Security;
using Microsoft.EntityFrameworkCore;

namespace BlogSystem.Infrastructure.Seed;

public class SeedData
{
    public static async Task InitializeAsync(
        BlogDbContext context,
        IPasswordHasher passwordHasher)
    {
        await context.Database.MigrateAsync();

        var adminExists = await context.Users
            .AnyAsync(x => x.Role == UserRole.Admin);

        if (adminExists)
            return;

        var admin = new User
        {
            FirstName = "Administrator",
            LastName = "Admin",
            UserName = "admin",
            Email = "admin@asterbit.ge",
            PasswordHash = passwordHasher.Hash("Adminadmin123."),
            Role = UserRole.Admin
        };

        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();
    }
}
