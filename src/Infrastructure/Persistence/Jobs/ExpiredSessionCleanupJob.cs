using Application.Common.Interfaces.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.Jobs;

public class ExpiredSessionCleanupJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ExpiredSessionCleanupJob(IServiceScopeFactory scopeFactory)
    {
        this._scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = this._scopeFactory.CreateScope();
            var sessionRepo = scope.ServiceProvider.GetRequiredService<ISessionRepository>();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await sessionRepo.DirectDeleteExpiredAsync(ct);
            await Task.Delay(TimeSpan.FromMinutes(30), ct);
        }
    }
}
