using IdentityServerApi.Host.Models.Responses;

namespace IdentityServerApi.Host.Services.Interfaces
{
    public interface IUserBffAccountService
    {
        public Task<GeneralResponse<List<SearchAdminUserResponse>>> AdminGetUsersByNameAsync(string userName);
        public Task<GeneralResponse<List<SearchUserResponse>>> UserGetUsersByNameAsync(string userName);
    }
}
