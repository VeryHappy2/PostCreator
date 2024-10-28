using Infrastructure.Configuration;

namespace Post.Host.Configurations;

internal class PostConfig
{
    public RateLimiter RateLimiter { get; set; }
    internal string PathBase { get; set; }
    internal string CdnHost { get; set; }
    internal string ConnectionString { get; set; }
}