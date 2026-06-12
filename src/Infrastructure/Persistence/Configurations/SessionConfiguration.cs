using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");

        // Id
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");

        // UserId
        builder.Property(s => s.UserId).HasColumnName("user_id");
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.UserId, s.CreatedAt })
            .HasDatabaseName("ix_sessions_user_id_created_at");

        // Roles
        builder.Ignore(c => c.Roles);
        builder.Property<List<Role>>("_roles")
            .HasColumnName("roles")
            .HasColumnType("text[]")
            .HasConversion(
                roles => roles.Select(r => r.ToString()).ToList(),
                strList => strList.Select(r => Enum.Parse<Role>(r)).ToList()
            )
            .Metadata.SetValueComparer(
                new ValueComparer<List<Role>>(
                    (a, b) => a != null && b != null && a.SequenceEqual(b),   // Equality
                    a => a.Aggregate(0, (prev, cur) => HashCode.Combine(prev, cur.GetHashCode())),  // Hashcode
                    a => a.ToList() // Copy
                )
            );

        // Token hash
        builder.OwnsOne(s => s.TokenHash, nav =>
        {
            nav.Property(h => h.Value)
                .HasColumnName("token_hash")
                .HasMaxLength(128)
                .IsRequired();
        });

        // ExpiresAt
        builder.Property(s => s.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        // LastSeen
        builder.Property(s => s.LastSeen)
            .HasColumnName("last_seen")
            .IsRequired();

        // CreatedAt
        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
    }
}
