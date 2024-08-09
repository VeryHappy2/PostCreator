using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerApi.Host.Repositories
{
    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        private readonly ApplicationDbContext _context;

        public UserAuthenticationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshTokenEntity> GetByIdRefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null!;
            }

            return await _context.RefreshToken.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }
    }
}
