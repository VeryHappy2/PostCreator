using Microsoft.AspNetCore.Identity;

namespace IdentityServerApi.Host.Data.Entities;

public class UserEnity : IdentityUser
{
    public string? Name { get; set; }
}