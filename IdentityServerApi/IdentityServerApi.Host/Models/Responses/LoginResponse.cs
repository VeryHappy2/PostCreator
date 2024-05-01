namespace IdentityServerApi.Host.Models.Responses
{
    public record class LoginResponse(bool Flag, string Token, string Message);
}
