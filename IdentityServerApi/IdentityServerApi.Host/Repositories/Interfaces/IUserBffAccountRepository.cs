using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Responses;

namespace IdentityServerApi.Host.Repositories.Interfaces
{
    public interface IUserBffAccountRepository
    {
        public Task<GeneralResponse<List<UserResponse>>> GetUsersByNameAsync(string userName);
    }
}
