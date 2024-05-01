namespace IdentityServerApi.Host.Models.Requests
{
    public class ChangeRoleRequest
    {
        public string Role { get; set; }
        public string UserName { get; set; }
    }
}
