using Domain.Common.Models;
using Domain.Entities;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly PublishDomainEventsInteceptor? _publishDomainEventsInteceptor;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        PublishDomainEventsInteceptor? publishDomainEventsInteceptor = null)
        : base(options)
    {
        this._publishDomainEventsInteceptor = publishDomainEventsInteceptor;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    // public DbSet<StandingOrder> StandingOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Sonst behandelt EFCore das aus irgendeinen grund als Entity
        /*modelBuilder.Ignore<MatchingRule>();
        modelBuilder.Ignore<Email>();
        modelBuilder.Ignore<Hash>();*/

        modelBuilder
            .Ignore<ValueObject>()
            .Ignore<IDomainEvent>();


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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (this._publishDomainEventsInteceptor != null)
            optionsBuilder.AddInterceptors(this._publishDomainEventsInteceptor);
        base.OnConfiguring(optionsBuilder);
    }
}
