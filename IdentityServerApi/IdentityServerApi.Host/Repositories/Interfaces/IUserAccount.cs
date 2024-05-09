using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;

namespace IdentityServerApi.Host.Repositories.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<GeneralResponse> CreateUserAccountAsync(UserRequest userDTO);
        Task<LoginResponse> LoginAccountAsync(LoginRequest loginDTO);
        Task<GeneralResponse> AddRoleAccountAsync(RoleRequest changeRoleRequest);
        Task<GeneralResponse> RemoveRoleAccountAsync(RoleRequest changeRoleRequest);
    }
}
