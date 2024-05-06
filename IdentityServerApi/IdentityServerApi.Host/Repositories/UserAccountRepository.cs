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
    public class UserAccountRepository(SignInManager<UserEnity> signInManager, UserManager<UserEnity> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : IUserAccountRepository
    {
        public async Task<GeneralResponse> ChangeRoleAccount(ChangeRoleRequest changeRoleRequest)
        {
            if (changeRoleRequest.Role != AuthRoles.Admin || changeRoleRequest.Role != AuthRoles.User)
                return new GeneralResponse(false, $"Such role: {changeRoleRequest.Role}, doesn't exist");

            var user = await userManager.FindByNameAsync(changeRoleRequest.UserName);

            if (user == null)
                return new GeneralResponse(false, "User not found");

            await userManager.AddToRoleAsync(user, changeRoleRequest.Role);

            return new GeneralResponse(true, $"Role was change in {changeRoleRequest.UserName}");
        }

        public async Task<GeneralResponse> CreateUserAccount(UserRequest userRequest)
        {
            if (userRequest is null)
                return new GeneralResponse(false, "Model is empty");

            var newUser = new UserEnity()
            {
                Name = userRequest.Name,
                Email = userRequest.Email,
                PasswordHash = userRequest.Password,
                UserName = userRequest.Email
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

        public async Task<LoginResponse> LoginAccount(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return new LoginResponse(false, null!, "Login container is empty");

            var getUser = await userManager.FindByEmailAsync(loginRequest.Email);
            if (getUser is null)
                return new LoginResponse(false, null!, "User not found");

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginRequest.Password);
            if (!checkUserPasswords)
                return new LoginResponse(false, null!, "Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);

            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!, "Login completed");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] !));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
