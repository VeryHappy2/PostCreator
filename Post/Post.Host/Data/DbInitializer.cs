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
			new PostCategoryEntity() { Category = "Pets" },
			new PostCategoryEntity() { Category = "Food" },
			new PostCategoryEntity() { Category = "Cars" }
		};
	}

	private static IEnumerable<PostItemEntity> GetPreconfiguredItems()
	{
		return new List<PostItemEntity>()
		{
			new PostItemEntity { Date = DateTime.Now.Date.ToUniversalTime(), CategoryId = 1, Content = "I like cats", UserId = "example", Title = "Cats", Comments = new List<PostCommentEntity>(), UserName = "UserName" },
		};
	}
}