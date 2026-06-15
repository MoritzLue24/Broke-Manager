using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MatchingRuleConfiguration : IEntityTypeConfiguration<MatchingRule>
{
    public void Configure(EntityTypeBuilder<MatchingRule> builder)
    {
        builder.ToTable("matching_rules");

        // Id
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        // FK wird über shadow property gesetzt
        builder.Property<Guid>("category_id");

        // Keyword
        builder.Property(r => r.Keyword)
            .HasColumnName("keyword")
            .HasMaxLength(255);
    }
}
