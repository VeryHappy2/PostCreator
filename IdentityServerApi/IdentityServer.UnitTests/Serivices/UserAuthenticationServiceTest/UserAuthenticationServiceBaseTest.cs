using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Repositories.Interfaces;
using IdentityServerApi.Host.Services;
using IdentityServerApi.Host.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace IdentityServer.UnitTests.Serivces.UserAuthenticationServiceTest
{
    public class UserAuthenticationServiceBaseTest
    {
        protected readonly IUserAuthenticationService UserAuthenticationService;

        protected readonly Mock<UserManager<UserApp>> UserManager;
        protected readonly IConfiguration Config;
        protected readonly ApplicationDbContext Context;
        protected readonly Mock<IUserAuthenticationRepository> UserAuthenticationRepository;

        private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _logger;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        public UserAuthenticationServiceBaseTest()
        {
            _logger = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            var dbContextTransaction = new Mock<IDbContextTransaction>();

            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            var userStore = new Mock<IUserStore<UserApp>>();

            var inMemorySettings = new Dictionary<string, string>
            {
                { "ConnectionString", "server=localhost;port=5435;database=identityserver;uid=postgres;password=postgres;" },
                { "Jwt:Key", "YcxjOMewdFfeZFQm5iGAYxTjR23Z93rLbyZucty3" },
                { "Jwt:Issuer", "http://www.postcreator.com:5100" },
                { "Jwt:Audience", "http://www.postcreator.com:5101" },
            };

            Config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            UserAuthenticationRepository = new Mock<IUserAuthenticationRepository>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            Context = new ApplicationDbContext(options);

            UserManager = new Mock<UserManager<UserApp>>(
                userStore.Object,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!,
                null!);

            UserAuthenticationService = new UserAuthenticationService(
                _dbContextWrapper.Object,
                _logger.Object,
                UserManager.Object,
                Config,
                Context,
                UserAuthenticationRepository.Object);
        }
    }
}
