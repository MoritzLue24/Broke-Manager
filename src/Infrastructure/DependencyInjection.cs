using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    /// Used for runtime, not design-time
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddPersistence(configuration);
        services.AddSecurity(configuration);
        return services;
    }

    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Default connection string not set"),
            builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddSecurity(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSection = configuration.GetSection(JwtSettings.SectionName);
        if (!jwtSection.Exists())
            throw new InvalidOperationException($"Jwt-settings section '{JwtSettings.SectionName}' is not set.");

        services.AddOptions<JwtSettings>()
            .Bind(jwtSection)
            .Validate(JwtSettings.Validate)
            .ValidateOnStart();

        // Signleton because we do not have a state, just one instance for all injections is enough 
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IHasher, Hasher>();

        return services;
    }
}
