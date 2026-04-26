using Microsoft.EntityFrameworkCore;
using Api.Models;


namespace Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Keyword> Keywords { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Email).HasMaxLength(255);
                e.Property(u => u.PasswordHash).HasMaxLength(255);
                e.Property(u => u.Role).HasConversion<string>().HasMaxLength(128);

                // User-Transaction, one-to-many relation
                e.HasMany(u => u.Transactions)
                    .WithOne(t => t.User)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // User-Category, one-to-many relation
                e.HasMany(u => u.Categories)
                    .WithOne(c => c.User)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Transaction>(e =>
            {
                e.HasKey(t => t.Id);
                e.Property(t => t.Date).HasConversion(
                    d => d.ToDateTime(TimeOnly.MinValue),   // C# to SQL
                    dSql => DateOnly.FromDateTime(dSql)     // SQL to C#
                ).HasColumnType("date");                    // Force columntype, to prevent datetime2

                e.Property(t => t.Amount).HasPrecision(12, 2);
                e.Property(t => t.CounterParty).HasMaxLength(255);
                e.Property(t => t.Title).HasMaxLength(500);

                // Transaction-User relation already configured

                // Transaction-Category, one-to-many relation
                e.HasOne(t => t.Category)
                    .WithMany()
                    .HasForeignKey(t => t.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Name).HasMaxLength(255);

                // Keywords
                e.HasMany(c => c.Keywords)
                    .WithOne(k => k.Category)
                    .HasForeignKey(k => k.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Store Interval as string, not as int (/enum in C#)
                e.Property(c => c.Interval)
                    .HasConversion<string>()
                    .HasMaxLength(128);

                // Category-User relation already configured
            });

            modelBuilder.Entity<Keyword>(e =>
            {
                e.HasKey(k => k.Id);
                e.Property(k => k.Value).HasMaxLength(500);

                // Keyword, Category relation already configured
            });
        }
    }
}