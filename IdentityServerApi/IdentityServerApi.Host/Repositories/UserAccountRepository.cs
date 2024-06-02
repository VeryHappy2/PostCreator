using IdentityModel;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Dtos;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Repositories.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServerApi.Host.Models.Contracts
{
    public class UserAccountRepository(
        SignInManager<UserApp> signInManager,
        UserManager<UserApp> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config) : IUserAccountRepository
    {
        public async Task<GeneralResponse> AddRoleAccountAsync(RoleRequest changeRoleRequest)
        {
            if (changeRoleRequest.Role != AuthRoles.Admin || changeRoleRequest.Role != AuthRoles.User)
                return new GeneralResponse(false, $"Such role: {changeRoleRequest.Role}, doesn't exist");

            var user = await userManager.FindByNameAsync(changeRoleRequest.UserName);
            if (user == null)
                return new GeneralResponse(false, "User not found");

            var userRoles = await userManager.GetRolesAsync(user);

            if (userRoles.Contains(changeRoleRequest.Role))
                return new GeneralResponse(false, "User already has such role");

            var result = await userManager.AddToRoleAsync(user, changeRoleRequest.Role);

            if (!result.Succeeded)
                return new GeneralResponse(false, "Error occured.. please try again");

            return new GeneralResponse(true, $"Role was change in {changeRoleRequest.UserName}");
        }

        public async Task<GeneralResponse> RemoveRoleAccountAsync(RoleRequest changeRoleRequest)
        {
            if (changeRoleRequest.Role != AuthRoles.Admin || changeRoleRequest.Role != AuthRoles.User)
                return new GeneralResponse(false, $"Such role: {changeRoleRequest.Role}, doesn't exist");

            var user = await userManager.FindByNameAsync(changeRoleRequest.UserName);

            if (user == null)
                return new GeneralResponse(false, "User not found");

            var userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains(changeRoleRequest.Role))
                return new GeneralResponse(false, "User already hasn't such role");

            var result = await userManager.RemoveFromRoleAsync(user, changeRoleRequest.Role);

            if (!result.Succeeded)
                return new GeneralResponse(false, "Error occured.. please try again");

            return new GeneralResponse(true, $"Role was change at {changeRoleRequest.UserName}");
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

            var userEmail = await userManager.FindByEmailAsync(newUser.Email);

            if (userEmail != null)
                return new GeneralResponse(false, "User registered already");

            var createUser = await userManager.CreateAsync(newUser!, userRequest.Password);

            if (!createUser.Succeeded)
                return new GeneralResponse(false, "Error occured.. please try again");

            var checkUser = await roleManager.FindByNameAsync(AuthRoles.User);

            if (checkUser == null)
                await roleManager.CreateAsync(new IdentityRole() { Name = AuthRoles.User });

            await userManager.AddToRoleAsync(newUser, AuthRoles.User);
            return new GeneralResponse(true, "Account Created");
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return new LoginResponse(false, null!, "Login request is empty");

            GeneralResponse<UserApp> response = await CheckUser(new CheckRequest
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password,
            });
            if (!response.Flag)
            {
                return new LoginResponse(false, null!, response.Message);
            }

            var getUserRole = await userManager.GetRolesAsync(response.Data);

            var userSession = new UserSession(response.Data.Id, response.Data.UserName, response.Data.Email, getUserRole.FirstOrDefault(x => x == AuthRoles.User));
            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!, "Login completed");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] !));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Name, user.Name),
                new Claim(JwtClaimTypes.Email, user.Email),
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
            var getUser = await userManager.FindByEmailAsync(user.Email);

            if (getUser == null)
                return new GeneralResponse<UserApp>(false, "Invalid email", null!);

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, user.Password);

            if (!checkUserPasswords)
                return new GeneralResponse<UserApp>(false, "Invalid password", null!);

            return new GeneralResponse<UserApp>(true, "Authentication successful", getUser);
        }
    }
}
