using Mapster;
using MapsterMapper;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        var mapsterConfig = TypeAdapterConfig.GlobalSettings;
        mapsterConfig.Scan(typeof(DependencyInjection).Assembly);   // Scans for IRegister & applies to the global config

        services.AddSingleton(mapsterConfig);   // Needed by mapster to inject into `ServiceMapper` (mapsters implementation of IMapper)
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddProblemDetails();   // To inject `IProblemDetailsService` to `ExceptionMiddleware`
        services.AddControllers();
        return services;
    }
}