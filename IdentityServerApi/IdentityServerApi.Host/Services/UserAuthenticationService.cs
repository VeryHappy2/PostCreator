using IdentityModel;
using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Dtos;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Repositories.Interfaces;
using IdentityServerApi.Host.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityServerApi.Host.Services
{
    public class UserAuthenticationService : BaseDataService<ApplicationDbContext>, IUserAuthenticationService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly IUserAuthenticationRepository _userAuthenticationRepository;

        public UserAuthenticationService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            UserManager<UserApp> userManager,
            IConfiguration config,
            ApplicationDbContext context,
            IUserAuthenticationRepository userAuthenticationRepository)
            : base(dbContextWrapper, logger)
        {
            _userManager = userManager;
            _config = config;
            _context = context;
            _userAuthenticationRepository = userAuthenticationRepository;
        }

        public async Task<GeneralResponse<string>> RefreshToken(string refreshToken, HttpContext context)
        {
            RefreshTokenEntity responseRefreshToken = await _userAuthenticationRepository.GetByRefreshToken(refreshToken);

            if (responseRefreshToken == null)
            {
                context.Response.Cookies.Delete("refresh-token");
                return new GeneralResponse<string>(false, "You need to log in again", null!);
            }

            UserApp user = await _userManager.FindByIdAsync(responseRefreshToken.UserId);

            if (user == null)
            {
                return new GeneralResponse<string>(false, "Refresh token is invalid", null!);
            }

            if (user.PasswordHash != responseRefreshToken.UserPasswordHash)
            {
                return new GeneralResponse<string>(false, "Password in refresh token is invalid", null!);
            }

            if (IsTokenExpired(responseRefreshToken.RefreshTokenExpiry))
            {
                return new GeneralResponse<string>(false, "You need to log in", null!);
            }

            var userRole = await _userManager.GetRolesAsync(user);

            string accessToken = GenerateToken(new UserSession(responseRefreshToken.UserId, user.UserName, userRole.First()));

            return new GeneralResponse<string>(true, "The new access token was created", accessToken);
        }

        public async Task<LoginResponse> LoginAccountAsync(LoginRequest loginRequest)
        {
            return await ExecuteSafeAsync(async () =>
            {
                if (loginRequest == null)
                    return new LoginResponse(false, null!, "Login request is empty", null!, null!);

                GeneralResponse<UserApp> response = await CheckUser(new CheckRequest
                {
                    Email = loginRequest.Email,
                    Password = loginRequest.Password,
                });

                if (!response.Flag)
                {
                    return new LoginResponse(false, null!, response.Message, null!, null!);
                }

                var getUserRole = await _userManager.GetRolesAsync(response.Data);

                string token = GenerateToken(
                    new UserSession(
                        response.Data.Id,
                        response.Data.UserName,
                        getUserRole.FirstOrDefault()));

                string refreshToken = GenerateRefreshTokenString();

                var result = await _context.RefreshToken.AddAsync(new RefreshTokenEntity
                {
                    RefreshToken = refreshToken,
                    UserPasswordHash = response.Data.PasswordHash,
                    UserId = response.Data.Id,
                });

                await _context.SaveChangesAsync();

                if (result.Entity.Id == null || result.Entity.Id == 0)
                {
                    return new LoginResponse(false, null!, "Refresh token didn't add", null!, null!);
                }

                return new LoginResponse(true, token!, "Login completed", refreshToken, new UserLoginResponse
                {
                    Id = response.Data.Id,
                    UserName = response.Data.UserName,
                    Role = getUserRole.FirstOrDefault(),
                });
            });
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] !));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Name, user.Name),
                new Claim(JwtClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        private async Task<GeneralResponse<UserApp>> CheckUser(CheckRequest user)
        {
            var checkUserEmail = await _userManager.FindByEmailAsync(user.Email);

            if (checkUserEmail == null)
                return new GeneralResponse<UserApp>(false, "Invalid email", null!);

            bool checkUserPasswords = await _userManager.CheckPasswordAsync(checkUserEmail, user.Password);

            if (!checkUserPasswords)
                return new GeneralResponse<UserApp>(false, "Invalid password", null!);

            return new GeneralResponse<UserApp>(true, "Authentication successful", checkUserEmail);
        }

        private bool IsTokenExpired(DateTime expiryDate)
        {
            return DateTime.UtcNow >= expiryDate;
        }
    }
}
