using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Api.IntegrationTests.TestInfrastructure.Controllers;

public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer
         = new PostgreSqlBuilder("postgres:latest")
            .WithDatabase("broke-manager-tests")
            .WithUsername("root-tests")
            .WithPassword("root-tests123!")
            .WithCleanUp(true)
            .WithAutoRemove(true)
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", this._postgresContainer.GetConnectionString());
        builder.UseSetting("JwtSettings:Secret", "test-secret-key-min-32-characters!!");
    }

    public async Task InitializeAsync()
    {
        await this._postgresContainer.StartAsync();

        var db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(this._postgresContainer.GetConnectionString())
            .Options);
        await db.Database.MigrateAsync();
        await db.DisposeAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await this._postgresContainer.DisposeAsync();
    }
}
