using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Infrastructure.Persistence.Repositories;
using Application.Common.Interfaces.Persistence;

namespace Infrastructure;

public static class DependencyInjection
{
    /// Used for runtime, not design-time
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
            connectionString,
            builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        ));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }
}
