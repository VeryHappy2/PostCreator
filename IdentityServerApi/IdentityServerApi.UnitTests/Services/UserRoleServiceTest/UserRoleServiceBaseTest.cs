using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Services;
using IdentityServerApi.Host.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityServerApi.UnitTests.Serivces.UserRoleServiceTest
{
    public class UserRoleServiceBaseTest
    {
        protected readonly IUserRoleService UserRoleService;

        protected readonly Mock<UserManager<UserApp>> UserManager;
        protected readonly Mock<RoleManager<IdentityRole>> RoleManager;
        public UserRoleServiceBaseTest()
        {
            var userStore = new Mock<IUserStore<UserApp>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

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

            UserRoleService = new UserRoleService(
                UserManager.Object,
                RoleManager.Object);
        }
    }
}
