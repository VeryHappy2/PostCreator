using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Services;
using IdentityServerApi.Host.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityServer.UnitTests.Serivces.UserAccountServiceTest
{
    public class UserAccountServiceBaseTest
    {
        protected readonly IUserAccountService UserAccountService;

        protected readonly Mock<UserManager<UserApp>> UserManager;
        protected readonly Mock<RoleManager<IdentityRole>> RoleManager;
        protected readonly IConfiguration Config;
        public UserAccountServiceBaseTest()
        {
            var userStore = new Mock<IUserStore<UserApp>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "YcxjOMewdFfeZFQm5iGAYxTjR23Z93rLbyZucty3" },
                { "Jwt:Issuer", "http://www.postcreator.com:5100" },
                { "Jwt:Audience", "http://www.postcreator.com:5101" },
            };

            Config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            UserManager = new Mock<UserManager<UserApp>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            RoleManager = new Mock<RoleManager<IdentityRole>>(
                roleStore.Object,
                null,
                null,
                null,
                null);

            UserAccountService = new UserAccountService(
                UserManager.Object,
                RoleManager.Object,
                Config);
        }
    }
}
