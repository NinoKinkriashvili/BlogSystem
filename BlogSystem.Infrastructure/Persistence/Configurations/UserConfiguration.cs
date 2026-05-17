using BlogSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSystem.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table name
        builder.ToTable("Users");

        // Primary Key
        builder.HasKey(x => x.Id);

        // BaseEntity fields
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        // FirstName
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        // LastName
        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(50);

        // UserName
        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(30);

        // Email
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        // PasswordHash
        builder.Property(x => x.PasswordHash)
            .IsRequired();

        // Role
        builder.Property(x => x.Role)
            .IsRequired()
            .HasConversion<int>(); // enum - int in DB
    }
}

