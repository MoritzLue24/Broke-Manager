using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    private class KeywordConverter : ValueConverter<Keyword, string>
    {
        public KeywordConverter()
            : base(k => k.Value, v => Keyword.Create(v).Value)
        { }
    }

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        // Id, kein isrequired nötig, autom.
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        // UserId
        builder.Property(c => c.UserId).HasColumnName("user_id");
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(c => c.UserId)
            .HasDatabaseName("ix_categories_user_id");

        // Name
        builder.Property(c => c.Name).HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();
        builder.HasIndex(c => new { c.UserId, c.Name }) // For unique category name
            .IsUnique()
            .HasDatabaseName("ix_categories_user_id_name");

        // IsDefault
        builder.Property(c => c.IsDefault).HasColumnName("is_default");

        // Keywords
        builder.PrimitiveCollection<List<Keyword>>("_keywords")
            .HasColumnName("keywords")
            .IsRequired()
        // Single keyword element
            .ElementType()
            .HasConversion(typeof(KeywordConverter))
            .HasMaxLength(255)
            .IsRequired();

        // CreatedAt
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");
    }
}