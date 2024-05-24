using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class AppDbContextExtensions
{
    public static void AddApplicationDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );
    }
}