using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zakerly.Domain.Interfaces.Repositories;
using Zakerly.Infrastructure.Persistence;
using Zakerly.Infrastructure.Repositories;
using Zakerly.Domain.Interfaces.Security;
using Zakerly.Infrastructure.Security;

namespace Zakerly.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
              ?? throw new InvalidOperationException(
                  "Connection string 'DefaultConnection' was not found.");
        services.AddDbContext<ZakerlyDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        return services;
    }
}