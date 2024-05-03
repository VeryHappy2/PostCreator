#pragma warning disable CS8618
namespace IdentityServerApi.Host.Configurations;

public class IdentityServerApiConfig
{
    public string CdnHost { get; set; }
    public string PathBase { get; set; }
    public string ConnectionString { get; set; }
}