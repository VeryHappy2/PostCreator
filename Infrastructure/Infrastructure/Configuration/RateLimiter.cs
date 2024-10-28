namespace Infrastructure.Configuration;

public class RateLimiter
{
    public int StatusCode { get; set; }
    public List<Policy> Policies { get; set; }
}