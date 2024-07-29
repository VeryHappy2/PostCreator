using FluentAssertions;
using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Repositories;
using IdentityServerApi.Host.Repositories.Interfaces;
using IdentityServerApi.Host.Services;
using IdentityServerApi.Host.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityServer.UnitTests.Serivces.UserAccountServiceTest
{
    public class UserBffAccountServiceBaseTest
    {
        protected readonly IUserBffAccountService UserBffAccountService;
        protected readonly Mock<IUserBffAccountRepository> UserBffAccountRepository;

        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _logger;

        public UserBffAccountServiceBaseTest()
        {
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();
            UserBffAccountRepository = new Mock<IUserBffAccountRepository>();

            UserBffAccountService = new UserBffAccountService(
                _dbContextWrapper.Object,
                _logger.Object,
                UserBffAccountRepository.Object);
        }
    }
}
