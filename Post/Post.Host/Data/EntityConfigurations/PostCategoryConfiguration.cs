using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Host.Data.Entities;

namespace Post.Host.Data.EntityConfigurations;

public class PostCategoryConfiguration
    : IEntityTypeConfiguration<PostCategoryEntity>
{
    public void Configure(EntityTypeBuilder<PostCategoryEntity> builder)
    {
        builder.ToTable("PostCategory");

        builder
            .HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .IsRequired();

        builder.Property(ci => ci.Catagory)
            .HasMaxLength(100)
            .IsRequired();
    }
}