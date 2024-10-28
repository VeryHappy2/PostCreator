using Microsoft.EntityFrameworkCore;
using Post.Host.Data.Entities;
using Post.Host.Data.EntityConfigurations;

namespace Post.Host.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PostItemEntity> PostItemEntity { get; set; }
    public DbSet<PostCommentEntity> PostCommentEntity { get; set; }
    public DbSet<PostCategoryEntity> PostCategoryEntity { get; set; }
    public DbSet<PostLikeEntity> PostLikeEntity { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PostEntityConfiguration());
        builder.ApplyConfiguration(new CommentEntityConfiguration());
        builder.ApplyConfiguration(new PostCategoryConfiguration());
        builder.UseHiLo();
    }
}
