using Post.Host.Data.Entities;

namespace Post.Host.Repositories.Interfaces
{
    public interface IPostBffRepository
    {
		Task<List<PostItemEntity>> GetPostsByUserIdAsync(string userId);
		Task<PostItemEntity> GetPostByIdAsync(int id);
	}
}
