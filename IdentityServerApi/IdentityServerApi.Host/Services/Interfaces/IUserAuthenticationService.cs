using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;

namespace IdentityServerApi.Host.Services.Interfaces
{
    public interface IUserAuthenticationService
    {
        Task<LoginResponse> LoginAccountAsync(LoginRequest loginRequest);
        Task<GeneralResponse<string>> RefreshToken(string refreshToken, HttpContext context);
    }
}
