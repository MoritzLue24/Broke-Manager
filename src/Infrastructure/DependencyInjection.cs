using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Infrastructure.Persistence.Repositories;
using Application.Features.Transactions;

namespace Infrastructure;

public static class DependencyInjection
{
    /// Used for runtime, not design-time
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryReaderRepository, CategoryReaderRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }
}
