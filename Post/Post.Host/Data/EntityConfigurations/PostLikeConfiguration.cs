using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Host.Data.Entities;

namespace Post.Host.Data.EntityConfigurations;

public class PostLikeConfiguration
    : IEntityTypeConfiguration<PostLikeEntity>
{
    public void Configure(EntityTypeBuilder<PostLikeEntity> builder)
    {
        builder.ToTable("PostCategory");

        builder
            .HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .IsRequired();

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.PostId)
            .IsRequired();
    }
}