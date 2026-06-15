using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        // Id
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        // UserId
        builder.Property(t => t.UserId).HasColumnName("user_id");
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("ix_transactions_user_id");


        // StandingOrderId, autom. nicht IsRequired, weil nullable
        builder.Property(t => t.StandingOrderId).HasColumnName("standing_order_id");
        // builder.HasOne<StandingOrder>()
        //     .WithMany()
        //     .HasForeignKey(t => t.StandingOrderId)
        //     .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(t => t.StandingOrderId)
            .HasFilter("\"standing_order_id\" IS NOT NULL")
            .HasDatabaseName("ix_transactions_standing_order_id");

        // CategoryId
        builder.Property(t => t.CategoryId).HasColumnName("category_id");
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(t => t.CategoryId)
            .HasDatabaseName("ix_transactions_category_id");

        // Sources
        builder.Property(t => t.CategorySource).HasColumnName("category_source")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(t => t.StandingOrderSource).HasColumnName("standing_order_source")
            .HasConversion<string>()
            .HasMaxLength(20);

        // Amount
        builder.Property(t => t.Amount).HasColumnName("amount")
            .HasColumnType("numeric(12,2)");

        builder.ToTable("transactions", t =>
            t.HasCheckConstraint("CK_transactions_amount_positive", "amount > 0")); //Amount nicht 0 und immer positiv

        // Type
        builder.Property(t => t.Type).HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Date
        builder.Property(t => t.Date).HasColumnName("date");
        builder.HasIndex(t => new { t.UserId, t.Date })
            .HasDatabaseName("ix_transactions_user_id_date");

        // Title
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
