using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Jobs;
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
        services.AddAuth(configuration);

        // Hasher
        services.AddSingleton<IHasher, Hasher>();

        return services;
    }

    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Default connection string not set");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString("DefaultConnection"),
            builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        ));

        services.AddScoped<PublishDomainEventsInteceptor>();    // Registered in AppDbContext
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddHostedService<ExpiredSessionCleanupJob>();

        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // Session settings
        var sessionSettingsSection = configuration.GetSection(SessionSettings.SectionName);
        if (!sessionSettingsSection.Exists())
            throw new InvalidOperationException($"Session-settings section '{SessionSettings.SectionName}' is not set.");

        // "Parses" the session section into an actual Session settings object
        var sessionSettings = sessionSettingsSection.Get<SessionSettings>()
            ?? throw new InvalidOperationException($"Session-settings section '{SessionSettings.SectionName}' could not be bound.");

        // It is possible that some properties of `sessionSettings` are null, if they are not set
        // so we need to validate
        if (!sessionSettings.Validate())
            throw new InvalidOperationException("Session-settings are invalid.");

        // Inject the session settings
        services.AddSingleton<ISessionSettings>(sessionSettings);

        services.AddScoped<ISessionCookieService, SessionCookieService>();

        // Token generator
        // Signleton because we do not have a state, just one instance for all injections is enough 
        services.AddSingleton<ISessionTokenGenerator, SessionTokenGenerator>();
        services.AddHttpContextAccessor();  // Because UserContext uses IHttpContextAccessor
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}
