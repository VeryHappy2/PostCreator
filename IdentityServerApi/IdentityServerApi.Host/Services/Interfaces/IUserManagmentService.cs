using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;

namespace IdentityServerApi.Host.Services.Interfaces
{
    public interface IUserManagmentService
    {
        Task<GeneralResponse> CreateUserAccountAsync(UserRequest userRequest);
        Task<GeneralResponse> DeleteUserAccountAsync(string userName);
    }
}
