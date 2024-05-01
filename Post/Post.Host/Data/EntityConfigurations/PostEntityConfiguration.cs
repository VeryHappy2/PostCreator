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
            .HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .IsRequired();

        builder.Property(ci => ci.UserId)
            .IsRequired();

        builder.Property(cb => cb.Title)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasMaxLength(3000)
            .IsRequired();

        builder.HasOne<PostCategoryEntity>(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);
    }
}