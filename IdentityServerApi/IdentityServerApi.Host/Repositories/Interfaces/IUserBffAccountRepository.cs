using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Responses;

namespace IdentityServerApi.Host.Repositories.Interfaces
{
    public interface IUserBffAccountRepository
    {
        public Task<GeneralResponse<List<SearchAdminUserResponse>>> AdminGetUsersByNameAsync(string userName);
        public Task<GeneralResponse<List<SearchUserResponse>>> UserGetUsersByNameAsync(string userName);
    }
}
