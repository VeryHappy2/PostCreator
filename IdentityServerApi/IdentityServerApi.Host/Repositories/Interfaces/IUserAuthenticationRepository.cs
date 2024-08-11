using IdentityServerApi.Host.Data.Entities;

namespace IdentityServerApi.Host.Repositories.Interfaces
{
    public interface IUserAuthenticationRepository
    {
        public Task<RefreshTokenEntity> GetByRefreshToken(string token);
    }
}
