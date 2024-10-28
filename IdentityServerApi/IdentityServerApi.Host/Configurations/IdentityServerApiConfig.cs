#pragma warning disable CS8618
using Infrastructure.Configuration;

namespace IdentityServerApi.Host.Configurations;

internal class IdentityServerApiConfig
{
    public RateLimiter RateLimiter { get; set; }
    internal string CdnHost { get; set; }
    internal string PathBase { get; set; }
    internal string ConnectionString { get; set; }
}