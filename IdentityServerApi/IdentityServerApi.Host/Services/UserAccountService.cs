using IdentityModel;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Dtos;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Services.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServerApi.Host.Services
{
    public class UserAccountService(
        UserManager<UserApp> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config) : IUserAccountService
    {
        public async Task<GeneralResponse> DeleteUserAccountAsync(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
                return new GeneralResponse(false, $"The {userName} wasn't found");

            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
                return new GeneralResponse(false, "Error occured.. please try again");

            return new GeneralResponse(true, $"The {user.UserName} was deleted");
        }

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
                return new GeneralResponse(false, $"Email: {userResponseFromEmail.UserName} registered already");

            var createUser = await userManager.CreateAsync(newUser!, userRequest.Password);

            if (!createUser.Succeeded)
                return new GeneralResponse(false, "Error occured.. please try again");

            await userManager.AddToRoleAsync(newUser, AuthRoles.User);
            return new GeneralResponse(true, "Account Created");
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return new LoginResponse(false, null!, "Login request is empty", null!);

            GeneralResponse<UserApp> response = await CheckUser(new CheckRequest
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password,
            });

            if (!response.Flag)
            {
                return new LoginResponse(false, null!, response.Message, null!);
            }

            var getUserRole = await userManager.GetRolesAsync(response.Data);

            string token = GenerateToken(
                new UserSession(
                    response.Data.Id,
                    response.Data.UserName,
                    getUserRole.FirstOrDefault()));

            return new LoginResponse(true, token!, "Login completed", new UserLoginResponse
            {
                Id = response.Data.Id,
                UserName = response.Data.UserName,
                Role = getUserRole.FirstOrDefault(),
            });
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

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] !));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Name, user.Name),
                new Claim(JwtClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<GeneralResponse<UserApp>> CheckUser(CheckRequest user)
        {
            var checkUserEmail = await userManager.FindByEmailAsync(user.Email);

            if (checkUserEmail == null)
                return new GeneralResponse<UserApp>(false, "Invalid email", null!);

            bool checkUserPasswords = await userManager.CheckPasswordAsync(checkUserEmail, user.Password);

            if (!checkUserPasswords)
                return new GeneralResponse<UserApp>(false, "Invalid password", null!);

            return new GeneralResponse<UserApp>(true, "Authentication successful", checkUserEmail);
        }
    }
}
