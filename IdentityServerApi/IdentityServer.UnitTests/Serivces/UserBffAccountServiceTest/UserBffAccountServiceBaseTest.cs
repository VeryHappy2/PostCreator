using FluentAssertions;
using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Repositories.Interfaces;
using IdentityServerApi.Host.Services;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityServer.UnitTests.Serivces.UserBffAccountServiceTest
{
    public class UserBffAccountServiceBaseTest
    {
        protected readonly Mock<IUserBffAccountRepository> _userBffAccountRepositoryMock;
        protected readonly UserBffAccountService _userBffAccountService;

        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapperMock;
        private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _loggerMock;

        public UserBffAccountServiceBaseTest()
        {
            _userBffAccountRepositoryMock = new Mock<IUserBffAccountRepository>();
            _dbContextWrapperMock = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _loggerMock = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();

            _dbContextWrapperMock.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _userBffAccountService = new UserBffAccountService(
                _dbContextWrapperMock.Object,
                _loggerMock.Object,
                _userBffAccountRepositoryMock.Object
            );
        }
    }
}
