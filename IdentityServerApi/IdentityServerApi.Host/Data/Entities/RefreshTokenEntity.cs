namespace IdentityServerApi.Host.Data.Entities
{
    public class RefreshTokenEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserPasswordHash { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; } = DateTime.UtcNow.AddDays(10);
    }
}
