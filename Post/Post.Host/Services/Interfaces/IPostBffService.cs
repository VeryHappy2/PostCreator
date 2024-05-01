using Post.Host.Models.Dtos;

namespace Post.Host.Services.Interfaces
{
    public interface IPostBffService
    {
		Task<List<PostItemDto>?> GetPostsByUserIdAsync(string userId);
		Task<PostItemDto?> GetPostByIdAsync(int id);
	}
}
