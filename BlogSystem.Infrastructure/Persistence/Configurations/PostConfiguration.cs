using BlogSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogSystem.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        // Table name
        builder.ToTable("Posts");

        // Primary Key
        builder.HasKey(x => x.Id);

        // BaseEntity fields
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.IsDeleted)
            .IsRequired();

        // Title
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        // Content
        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(5000);

        // FK
        builder.Property(x => x.UserId)
            .IsRequired();

        // Relationship: Post - User
        builder.HasOne(x => x.User)
            .WithMany(x => x.Posts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);  // User delete - post exist

        // Index (performance)
        builder.HasIndex(x => x.UserId);
    }
}
