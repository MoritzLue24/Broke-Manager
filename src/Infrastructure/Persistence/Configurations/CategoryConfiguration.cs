using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        // Id
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

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
        builder.HasIndex(c => c.UserId) // User has only 1 Default Category
            .HasFilter("is_default = true")
            .IsUnique()
            .HasDatabaseName("ix_categories_user_id_unique_default");

        // Matching rules
        builder.OwnsMany(c => c.MatchingRules, b =>
        {
            b.ToTable("matching_rules");
            b.UsePropertyAccessMode(PropertyAccessMode.Field);

            // Shadow FK, VO gets loaded with category
            b.WithOwner().HasForeignKey("category_id");

            // Shadow composite primary key
            b.HasKey("category_id", nameof(MatchingRule.Keyword));

            b.Property(r => r.Keyword)
                .IsRequired()
                .HasColumnName("keyword")
                .HasMaxLength(255);

        });


        /*
        builder.OwnsMany(c => c.MatchingRules, ruleBuilder =>
        {
            ruleBuilder.ToTable("matching_rules");
            ruleBuilder.WithOwner().HasForeignKey("category_id");
            ruleBuilder.Property(r => r.Keyword)
                .HasColumnName("keyword")
                .HasMaxLength(255)
                .IsRequired();
        });

        /*
        // Keywords
        builder.PrimitiveCollection<List<MatchingRule>>("_matchingRules")
            .HasColumnName("matching_rules")
            .IsRequired()
        // Single keyword element
            .ElementType()
            .HasConversion(typeof(MatchingRuleConverter))
            .HasMaxLength(255)
            .IsRequired();
        */

        // CreatedAt
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");
    }
}
