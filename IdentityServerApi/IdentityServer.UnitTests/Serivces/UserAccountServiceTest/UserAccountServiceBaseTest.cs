using IdentityServerApi.Host.Data;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Repositories.Interfaces;
using IdentityServerApi.Host.Services;
using IdentityServerApi.Host.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace IdentityServer.UnitTests.Serivces.UserAccountServiceTest
{
    public class UserAccountServiceBaseTest
    {
        protected readonly IUserAuthenticationService UserAuthenticationService;
        protected readonly IUserRoleService UserRoleService;
        protected readonly IUserManagmentService UserManagmentService;

        protected readonly Mock<UserManager<UserApp>> UserManager;
        protected readonly Mock<RoleManager<IdentityRole>> RoleManager;
        protected readonly IConfiguration Config;
        protected readonly ApplicationDbContext Context;
        protected readonly Mock<IUserAuthenticationRepository> UserAuthenticationRepository;
        public UserAccountServiceBaseTest()
        {
            var userStore = new Mock<IUserStore<UserApp>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

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

            RoleManager = new Mock<RoleManager<IdentityRole>>(
                roleStore.Object,
                null!,
                null!,
                null!,
                null!);

            UserAuthenticationService = new UserAuthenticationService(
                UserManager.Object,
                Config,
                Context,
                UserAuthenticationRepository.Object);

            UserRoleService = new UserRoleService(
                UserManager.Object,
                RoleManager.Object);

            UserManagmentService = new UserManagmentService(
                UserManager.Object);
        }
    }
}
