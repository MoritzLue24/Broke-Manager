using System.Text;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // Jwt settings
        var jwtSection = configuration.GetSection(JwtSettings.SectionName);
        if (!jwtSection.Exists())
            throw new InvalidOperationException($"Jwt-settings section '{JwtSettings.SectionName}' is not set.");

        var jwtSettings = jwtSection.Get<JwtSettings>()
            ?? throw new InvalidOperationException($"Jwt-settings section '{JwtSettings.SectionName}' could not be bound.");

        if (!jwtSettings.Validate())
            throw new InvalidOperationException("Jwt-settings are invalid.");

        services.AddSingleton(Options.Create(jwtSettings));

        // Token generator
        // Signleton because we do not have a state, just one instance for all injections is enough 
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // Token validator
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret)
                    )
                };
                options.Events = new JwtBearerEvents
                {
                    // Called first, before token is read
                    OnMessageReceived = ctx =>
                    {
                        var cookie = ctx.Request.Cookies[jwtSettings.CookieName];
                        ctx.Token = cookie;
                        return Task.CompletedTask;
                    }
                    // OnChallange set in api layer
                };
            });

        services.AddHttpContextAccessor();  // Because UserContext uses IHttpContextAccessor
        services.AddScoped<IUserContext, UserContext>();

        return services;
    }
}
