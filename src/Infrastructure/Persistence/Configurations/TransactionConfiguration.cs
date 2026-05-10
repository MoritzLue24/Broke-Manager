using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        // Id, kein isrequired nötig, autom.
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");

        // UserId, autom. IsRequired automatisch
        builder.Property(t => t.UserId).HasColumnName("user_id");
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => t.UserId)     // Häufige abfrage: all transactions of user
            .HasDatabaseName("ix_transactions_user_id");

        // StandingOrderId, autom. nicht IsRequired, weil nullable
        builder.Property(t => t.StandingOrderId).HasColumnName("standing_order_id");
        // builder.HasOne<StandingOrder>()
        //     .WithMany()
        //     .HasForeignKey(t => t.StandingOrderId)
        //     .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(t => t.StandingOrderId)    // Auch häufige abfrage
            .HasFilter("\"standing_order_id\" IS NOT NULL")
            .HasDatabaseName("ix_transactions_standing_order_id");

        // CategoryId
        builder.Property(t => t.CategoryId).HasColumnName("category_id");
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(t => t.CategoryId) // Häufige abfrage
            .HasDatabaseName("ix_transactions_category_id");

        // Sources, IsRequired weil als string gespeichert wird -> nicht autom. manchmal?
        builder.Property(t => t.CategorySource).HasColumnName("category_source")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(t => t.StandingOrderSource).HasColumnName("standing_order_source")
            .HasConversion<string>()
            .HasMaxLength(20);  // kein isRequired, weil ist nullable!

        // Amount
        builder.Property(t => t.Amount).HasColumnName("amount")
            .HasColumnType("numeric(12,2)");

        // Type
        builder.Property(t => t.Type).HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Date
        builder.Property(t => t.Date).HasColumnName("date");
        builder.HasIndex(t => new { t.UserId, t.Date }) // Auch häufige abfrage für zeiträume
            .HasDatabaseName("ix_transactions_user_id_date");

        // Title, IsRequired weil als string gespeichert wird -> nicht autom. manchmal?
        builder.Property(t => t.Title).HasColumnName("title")
            .HasMaxLength(255)
            .IsRequired();

        // Description
        builder.Property(t => t.Description).HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();

        // CounterParty
        builder.Property(t => t.CounterParty).HasColumnName("counter_party")
            .HasMaxLength(255)
            .IsRequired();

        // CreatedAt
        builder.Property(t => t.CreatedAt).HasColumnName("created_at");
    }
}