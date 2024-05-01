using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;

namespace IdentityServerApi.Host.Repositories.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<GeneralResponse> CreateUserAccount(UserRequest userDTO);
        Task<LoginResponse> LoginAccount(LoginRequest loginDTO);
        Task<GeneralResponse> ChangeRoleAccount(ChangeRoleRequest changeRoleRequest);
    }
}
