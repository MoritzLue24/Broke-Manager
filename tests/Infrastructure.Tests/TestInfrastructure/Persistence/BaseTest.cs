using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

// This namespace is called `TestInfrastructure` no matter what our testing project is.
// If we would test the api it is called `TestInfrastructure` anyway becasue
// this is the infrastructure we use for testing
namespace Infrastructure.Tests.TestInfrastructure.Persistence;

[Collection("PostgresCollection")]
public abstract class BaseTest : IAsyncLifetime
{
    protected readonly AppDbContext _dbContext;

    // Gets called before every test case
    public BaseTest(PostgresFixture postgres)
        => this._dbContext = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(postgres.ConnectionString)
            .Options
        );

    // Gets called before EVERY test case, but async
    public Task InitializeAsync()
        => Task.CompletedTask;

    // Gets called after every test case, async
    public async Task DisposeAsync()
        => await this._dbContext.Database.ExecuteSqlRawAsync(
            "TRUNCATE TABLE transactions, categories, users RESTART IDENTITY CASCADE"
        );
}