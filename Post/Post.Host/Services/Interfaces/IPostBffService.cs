using Catalog.Host.Models.Requests;
using Post.Host.Models.Dtos;
using Post.Host.Models.Response;

namespace Post.Host.Services.Interfaces
{
    public interface IPostBffService
    {
		Task<List<PostItemDto>?> GetPostsByUserIdAsync(string userId);
		Task<PostItemDto?> GetPostByIdAsync(int id);
		Task<PaginatedResponse<PostItemDto>> GetPostByPageAsync(PageItemRequest pageItemRequest);
		Task<List<PostCategoryDto>?> GetPostCategoriesAsync();
	}
}
