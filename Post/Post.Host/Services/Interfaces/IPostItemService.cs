using Post.Host.Models.Responses;

namespace Post.Host.Services.Interfaces
{
    public interface IPostItemService
    {
        public Task<GeneralResponse> DeleteByUserIdAsync(string userId);
        public Task<GeneralResponse> AddViewAsync(int id);
    }
}