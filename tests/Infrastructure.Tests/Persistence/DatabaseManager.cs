using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Persistence;

public class DatabaseManager
{
    public AppDbContext Context { get; private set; }

    public DatabaseManager(string connectionString)
    {
        this.Context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(connectionString)
            .Options
        );
    }

    /// Reset the database for every testcase to ensure a clean db
    public async Task ResetAsync()
    {
        await this.Context.Database.EnsureDeletedAsync();
        await this.Context.Database.MigrateAsync();
    }
}
