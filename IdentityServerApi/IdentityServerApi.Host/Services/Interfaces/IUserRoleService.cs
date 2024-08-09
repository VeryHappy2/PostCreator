using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;

namespace IdentityServerApi.Host.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<GeneralResponse<List<string>>> GetRolesAsync();
        Task<GeneralResponse> ChangeRoleAccountAsync(RoleRequest changeRoleRequest);
    }
}
