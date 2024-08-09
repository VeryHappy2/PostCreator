namespace IdentityServerApi.Host.Models.Responses
{
    public record class LoginResponse(bool Flag, string AccessToken, string Message, string RefreshToken, UserLoginResponse User);
}
