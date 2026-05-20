using BlogSystem.Application.Interfaces.Repositories;
using BlogSystem.Application.Interfaces.Security;
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

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IJwtService>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();

            return new JwtService(
                config["Jwt:Key"]!,
                config["Jwt:Issuer"]!,
                config["Jwt:Audience"]!,
                int.Parse(config["Jwt:Expires"]!)
            );
        });

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
