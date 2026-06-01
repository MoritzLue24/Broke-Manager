using Application.Common.Interfaces.Security;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests.TestInfrastructure.Controllers;

[Collection("WebAppCollection")]
public abstract class BaseTest : IAsyncLifetime
{
    private readonly IServiceScope _scope;
    protected HttpClient Client { get; private init; }
    protected AppDbContext Db { get; private init; }
    protected IJwtTokenGenerator JwtTokenGenerator { get; private init; }

    protected BaseTest(WebAppFactory factory)
    {
        // We need to create a scope manually, because the database is created as a scoped service
        // Normally, aspdotnet handles that for us
        this._scope = factory.Services.CreateScope();
        this.Client = factory.CreateClient();
        this.Db = this._scope.ServiceProvider.GetRequiredService<AppDbContext>();   // because scoped
        this.JwtTokenGenerator = factory.Services.GetRequiredService<IJwtTokenGenerator>(); // because singleton
    }

    public Task InitializeAsync()
        => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await this.Db.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE transactions, categories, users RESTART IDENTITY CASCADE"
        );
        this._scope.Dispose();
    }
}
