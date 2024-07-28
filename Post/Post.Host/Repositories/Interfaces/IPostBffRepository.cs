using Catalog.Host.Models.Requests;
using Post.Host.Data;
using Post.Host.Data.Entities;

namespace Post.Host.Repositories.Interfaces
{
    public interface IPostBffRepository
    {
		Task<List<PostItemEntity>?> GetPostsByUserIdAsync(string userId);
		Task<PostItemEntity?> GetPostByIdAsync(int id);
		Task<PaginatedItems<PostItemEntity>> GetByPageAsync(PageItemRequest pageItemRequest);
		Task<List<PostCategoryEntity>> GetPostCategoriesAsync();
	}
}
