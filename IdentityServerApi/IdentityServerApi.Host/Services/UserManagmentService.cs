using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerApi.Host.Services
{
    public class UserManagmentService(
        UserManager<UserApp> userManager) : IUserManagmentService
    {
        public async Task<GeneralResponse> CreateUserAccountAsync(UserRequest userRequest)
        {
            if (userRequest is null)
                return new GeneralResponse(false, "Model is empty");

            var newUser = new UserApp()
            {
                Email = userRequest.Email,
                PasswordHash = userRequest.Password,
                UserName = userRequest.Name,
            };

            var userResponseFromName = await userManager.FindByNameAsync(userRequest.Name);

            if (userResponseFromName != null)
                return new GeneralResponse(false, $"User: {userResponseFromName.UserName} registered already");

            var userResponseFromEmail = await userManager.FindByEmailAsync(newUser.Email);

            if (userResponseFromEmail != null)
                return new GeneralResponse(false, $"Email: {userResponseFromEmail.Email} registered already");

            var createUser = await userManager.CreateAsync(newUser!, userRequest.Password);

            if (!createUser.Succeeded)
                return new GeneralResponse(false, "Error occured.. please try again");

            await userManager.AddToRoleAsync(newUser, AuthRoles.User);
            return new GeneralResponse(true, "Account Created");
        }

        public async Task<GeneralResponse<string>> DeleteUserAccountAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
                return new GeneralResponse<string>(false, $"The {userName} wasn't found", null);

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return new GeneralResponse<string>(false, "Error occured.. please try again", null);

            return new GeneralResponse<string>(true, $"The {user.UserName} was deleted", user.Id);
        }
    }
}
