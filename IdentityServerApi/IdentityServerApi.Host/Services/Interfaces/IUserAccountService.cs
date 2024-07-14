using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerApi.Host.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<GeneralResponse<List<string>>> GetRoles();
        Task<GeneralResponse> CreateUserAccountAsync(UserRequest userDTO);
        Task<GeneralResponse> DeleteUserAccountAsync(string userName);
        Task<LoginResponse> LoginAccountAsync(LoginRequest loginDTO);
        Task<GeneralResponse> ChangeRoleAccountAsync(RoleRequest changeRoleRequest);
    }
}
