namespace IdentityServerApi.Host.Models.Responses
{
    public class SearchAdminUserResponse
    {
        required public string RoleName { get; init; }
        required public string UserName { get; init; }
    }
}
