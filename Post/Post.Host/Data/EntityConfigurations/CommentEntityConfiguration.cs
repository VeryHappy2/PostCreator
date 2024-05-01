using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Post.Host.Data.Entities;

namespace Post.Host.Data.EntityConfigurations;

public class CommentEntityConfiguration
    : IEntityTypeConfiguration<PostCommentEntity>
{
    public void Configure(EntityTypeBuilder<PostCommentEntity> builder)
    {
        builder.ToTable("PostComment");

        builder
            .HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .IsRequired();

        builder.Property(cb => cb.PostId)
            .IsRequired();

        builder.Property(x => x.Content)
            .IsRequired();

        builder
            .HasOne<PostItemEntity>()
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.PostId);
    }
}