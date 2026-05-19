using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    // public DbSet<StandingOrder> StandingOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Sonst behandelt EFCore das aus irgendeinen grund als Entity
        modelBuilder.Ignore<Keyword>();
        modelBuilder.Ignore<Email>();
        modelBuilder.Ignore<Hash>();
        // TODO, Besser:
        // var valueObjectTypes = typeof(Keyword).Assembly
        //     .GetTypes()
        //     .Where(t => typeof(IValueObject).IsAssignableFrom(t) && t.IsClass);

        // foreach (var type in valueObjectTypes)
        //     modelBuilder.Ignore(type);
        // Und alle VOs in Domain implementen IValueObject

        // Lädt alle Classes, die in diesen Projekt IEntityTypeConfiguration implementieren
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
