using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
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

    /// Used for Design-Time, without DI / Program.cs
    /// For example when running just migrate
    public AppDbContext CreateDbContext(string[] args)
    {
        LoadNearestEnv();

        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST")
            ?? throw new InvalidOperationException("POSTGRES_HOST not set");

        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT")
            ?? throw new InvalidOperationException("POSTGRES_PORT not set");

        var user = Environment.GetEnvironmentVariable("POSTGRES_USER")
            ?? throw new InvalidOperationException("POSTGRES_USER not set");

        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
            ?? throw new InvalidOperationException("POSTGRES_PASSWORD not set");

        var db = Environment.GetEnvironmentVariable("POSTGRES_DB")
            ?? throw new InvalidOperationException("POSTGRES_DB not set");

        return new(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql($"Host={host};Port={port};Database={db};Username={user};Password={password};Trust Server Certificate=true")
                .Options
        );
    }
}