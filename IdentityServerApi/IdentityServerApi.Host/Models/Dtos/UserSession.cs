﻿namespace IdentityServerApi.Host.Models.Dtos
{
    public record UserSession(string? Id, string? Name, string? Email, IList<string?> Role);
}
