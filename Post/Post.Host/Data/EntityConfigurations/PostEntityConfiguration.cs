using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Host.Data.Entities;

namespace Post.Host.Data.EntityConfigurations;

public class PostEntityConfiguration
    : IEntityTypeConfiguration<PostItemEntity>
{
    public void Configure(EntityTypeBuilder<PostItemEntity> builder)
    {
        builder.ToTable("PostItem");

        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.UserName)
            .IsRequired();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasMaxLength(3000)
            .IsRequired();

        builder.Property(x => x.Views)
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasOne<PostCategoryEntity>(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);

        builder.HasMany<PostLikeEntity>(x => x.Likes)
            .WithOne()
            .HasForeignKey(x => x.PostId);
    }
}