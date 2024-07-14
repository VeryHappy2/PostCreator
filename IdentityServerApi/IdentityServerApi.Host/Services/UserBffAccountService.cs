using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Repositories.Interfaces;
using IdentityServerApi.Host.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

namespace IdentityServerApi.Host.Services
{
    public class UserBffAccountService : BaseDataService<ApplicationDbContext>, IUserBffAccountService
    {
        private readonly IUserBffAccountRepository _userBffAccountRepository;
        public UserBffAccountService(
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<BaseDataService<ApplicationDbContext>> logger,
            IUserBffAccountRepository userBffAccountRepository)
            : base(dbContextWrapper, logger)
        {
            _userBffAccountRepository = userBffAccountRepository;
        }

        public async Task<GeneralResponse<List<UserResponse>>> GetUsersByNameAsync(string userName)
        {
            return await ExecuteSafeAsync(async () =>
            {
                return await _userBffAccountRepository.GetUsersByNameAsync(userName);
            });
        }
    }
}
