using Post.Host.Data.Entities;
using Post.Host.Models.Responses;

namespace Post.Host.Repositories.Interfaces
{
    public interface IPostItemRepository
    {
        Task<GeneralResponse> DeleteByUserIdAsync(string userId);
        Task<PostItemEntity> GetByIdAsync(int id);
    }
}
