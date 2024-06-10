using Post.Host.Data;
using Post.Host.Data.Entities;

namespace Post.Host.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        if (!context.PostCategoryEntity.Any())
        {
            await context.PostCategoryEntity.AddRangeAsync(GetPreconfiguredPostCategory());

            await context.SaveChangesAsync();
        }

        if (!context.PostItemEntity.Any())
        {
            await context.PostItemEntity.AddRangeAsync(GetPreconfiguredItems());

            await context.SaveChangesAsync();
        }
    }

	private static IEnumerable<PostCategoryEntity> GetPreconfiguredPostCategory()
	{
		return new List<PostCategoryEntity>()
		{
			new PostCategoryEntity() { Category = "pets" },
		};
	}

	private static IEnumerable<PostItemEntity> GetPreconfiguredItems()
	{
		return new List<PostItemEntity>()
		{
			new PostItemEntity { Date = DateTime.Now.Date.ToUniversalTime(), CategoryId = 1, Content = "Super content", UserId = "example", Title = "Super post", Comments = new List<PostCommentEntity>() },
		};
	}
}