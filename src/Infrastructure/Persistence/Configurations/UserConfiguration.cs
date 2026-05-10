using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        // Id
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        // Email
        builder.OwnsOne(u => u.Email, nav =>
        {
            nav.Property(email => email.Value)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();
            nav.HasIndex(email => email.Value)
                .IsUnique()
                .HasDatabaseName("ix_users_email");
        });

        // PasswordHash
        builder.OwnsOne(u => u.PasswordHash, nav =>
        {
            nav.Property(h => h.Value)
                .HasColumnName("password_hash")
                .HasMaxLength(128)
                .IsRequired();
        });

        // Role
        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // CreatedAt
        builder.Property(u => u.CreatedAt).HasColumnName("created_at");
    }
}