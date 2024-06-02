namespace IdentityServerApi.Host.Models.Requests;

public class CheckRequest
{
    required public string Email { get; set; }
    required public string Password { get; set; }
}