using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DependencyInjection;

public static class ServicesExtension
{
    /// Used for runtime, not design-time
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection serviceCollection,
        string connectionString)
    {
        serviceCollection.AddDbContext<AppDbContext>(
            options => options.UseNpgsql(connectionString)
        );
        return serviceCollection;
    }
}