using Testcontainers.PostgreSql;

namespace Infrastructure.Tests.Persistence;

/// Lives across all tests, created & injected by xunit because IAsyncLifetime
/// Creates and handles a postgreSql container,
/// but not the database context. -> DatabaseManager
public class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; private set; } = null!;
    public string ConnectionString => Container.GetConnectionString();

    /// Searches recursivly the folders above for a .env file and applies it
    private static void LoadNearestEnv()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, ".env")))
        {
            Console.WriteLine($"Searching for .env in '{dir}'");
            dir = dir.Parent;
        }

        if (dir == null)
            throw new InvalidOperationException(".env file not found");

        Console.WriteLine($"Found .env in '{dir}'");
        DotNetEnv.Env.Load(Path.Combine(dir.FullName, ".env"));
    }

    public async Task InitializeAsync()
    {
        LoadNearestEnv();

        var user = Environment.GetEnvironmentVariable("POSTGRES_USER")
            ?? throw new InvalidOperationException("POSTGRES_USER not set");

        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
            ?? throw new InvalidOperationException("POSTGRES_PASSWORD not set");

        var db = Environment.GetEnvironmentVariable("POSTGRES_DB")
            ?? throw new InvalidOperationException("POSTGRES_DB not set");

        Container = new PostgreSqlBuilder("postgres:latest")
            .WithDatabase(db)
            .WithUsername(user)
            .WithPassword(password)
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