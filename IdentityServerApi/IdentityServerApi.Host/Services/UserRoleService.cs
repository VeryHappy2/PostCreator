using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerApi.Host.Services
{
    public class UserRoleService(
        UserManager<UserApp> userManager,
        RoleManager<IdentityRole> roleManager) : IUserRoleService
    {
        public async Task<GeneralResponse> ChangeRoleAccountAsync(RoleRequest changeRoleRequest)
        {
            if (!AuthRoles.AllRoles.Contains(changeRoleRequest.Role))
                return new GeneralResponse(false, $"Such role: {changeRoleRequest.Role}, doesn't exist");

            var user = await userManager.FindByNameAsync(changeRoleRequest.UserName);

            if (user == null)
                return new GeneralResponse(false, "User not found");

            var userRoles = await userManager.GetRolesAsync(user);

            if (userRoles.Contains(changeRoleRequest.Role))
                return new GeneralResponse(false, "User already has such a role");

            var removeResponse = await userManager.RemoveFromRoleAsync(user, userRoles.FirstOrDefault());

            if (!removeResponse.Succeeded)
                return new GeneralResponse(false, "User hasn't any role");

            var result = await userManager.AddToRoleAsync(user, changeRoleRequest.Role);

            if (!result.Succeeded)
                return new GeneralResponse(false, "Error occured.. please try again");

            return new GeneralResponse(true, $"Role was change in {changeRoleRequest.UserName}");
        }

        public async Task<GeneralResponse<List<string>>> GetRolesAsync()
        {
            var roles = await roleManager.Roles.Select(x => x.Name).ToListAsync();

            if (roles == null)
            {
                return new GeneralResponse<List<string>>(false, "Not found any roles", null!);
            }

            return new GeneralResponse<List<string>>(true, "Successfully", roles);
        }
    }
}
