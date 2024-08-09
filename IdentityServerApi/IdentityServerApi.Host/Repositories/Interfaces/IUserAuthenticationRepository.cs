using IdentityServerApi.Host.Data.Entities;

namespace IdentityServerApi.Host.Repositories.Interfaces
{
    public interface IUserAuthenticationRepository
    {
        public Task<RefreshTokenEntity> GetByIdRefreshToken(string token);
    }
}
