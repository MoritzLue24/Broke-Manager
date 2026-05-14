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

        return new(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"))
                .Options
        );
    }
}
