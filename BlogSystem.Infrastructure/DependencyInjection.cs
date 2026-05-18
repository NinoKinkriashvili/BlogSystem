using BlogSystem.Application.Interfaces.Repositories;
using BlogSystem.Infrastructure.Persistence;
using BlogSystem.Infrastructure.Persistence.Repositories;
using BlogSystem.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BlogDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();

        services.AddSingleton<PasswordHasher>();
        services.AddScoped<JwtService>();

        return services;
    }
}
