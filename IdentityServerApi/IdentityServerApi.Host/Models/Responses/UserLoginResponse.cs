namespace IdentityServerApi.Host.Models.Responses
{
    public class UserLoginResponse
    {
        required public string Id { get; init; }
        required public string UserName { get; init; }
        required public string Role { get; init; }
    }
}
