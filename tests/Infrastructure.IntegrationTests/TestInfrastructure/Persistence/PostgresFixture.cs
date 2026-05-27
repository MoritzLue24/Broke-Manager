using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Infrastructure.IntegrationTests.TestInfrastructure.Persistence;

/// Lives across all tests, created
/// Creates and handles a postgreSql container,
/// but not the database context. -> DatabaseManager
public class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; private set; }
        = new PostgreSqlBuilder("postgres:latest")
            .WithDatabase("broke-manager-tests")
            .WithUsername("root-tests")
            .WithPassword("root-tests123!")
            .WithCleanUp(true)
            .WithAutoRemove(true)
            .Build();

    public string ConnectionString => this.Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await this.Container.StartAsync();

        var db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(this.ConnectionString)
            .Options);
        await db.Database.MigrateAsync();
        await db.DisposeAsync();
    }

    public async Task DisposeAsync()
    {
        if (this.Container != null)
            await this.Container.DisposeAsync();
    }
}
