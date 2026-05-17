using Testcontainers.PostgreSql;

namespace Infrastructure.Tests.Persistence;

/// Lives across all tests, created & injected by xunit because IAsyncLifetime
/// Creates and handles a postgreSql container,
/// but not the database context. -> DatabaseManager
public class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; private set; } = null!;
    public string ConnectionString => Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        Container = new PostgreSqlBuilder("postgres:latest")
            .WithDatabase("broke-manager-tests")
            .WithUsername("root-tests")
            .WithPassword("root-tests123!")
            .WithCleanUp(true)
            .WithAutoRemove(true)
            .Build();
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (Container != null)
            await Container.DisposeAsync();
    }
}