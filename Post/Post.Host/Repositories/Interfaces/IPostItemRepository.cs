using Post.Host.Models.Responses;

namespace Post.Host.Repositories.Interfaces
{
    public interface IPostItemRepository
    {
        Task<GeneralResponse> DeleteByUserIdAsync(string userId);
    }
}
