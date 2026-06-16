using Application.Features.Auth.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Jobs;

public class SessionCleanupJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SessionCleanupJob(IServiceScopeFactory scopeFactory)
    {
        this._scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = this._scopeFactory.CreateScope();
            var sessionRepo = scope.ServiceProvider.GetRequiredService<ISessionRepository>();

            await sessionRepo.ExecuteDeleteExpiredAsync(ct);
            await Task.Delay(TimeSpan.FromMinutes(30), ct);
        }
    }
}
