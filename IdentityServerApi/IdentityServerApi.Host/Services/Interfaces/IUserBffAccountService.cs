using IdentityServerApi.Host.Models.Responses;

namespace IdentityServerApi.Host.Services.Interfaces
{
    public interface IUserBffAccountService
    {
        public Task<GeneralResponse<List<UserResponse>>> GetUsersByNameAsync(string userName);
    }
}
