using FluentAssertions;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using IdentityServerApi.Host.Services;
using IdentityServerApi.Host.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace IdentityServer.UnitTests.Serivces
{
    public class UserAccountServiceTest
    {
        private readonly IUserAccountService _userAccountService;

        private readonly Mock<UserManager<UserApp>> _userManager;
        private readonly Mock<RoleManager<IdentityRole>> _roleManager;
        private readonly Mock<IConfiguration> _config;
        public UserAccountServiceTest()
        {
            var userStore = new Mock<IUserStore<UserApp>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            _userManager = new Mock<UserManager<UserApp>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            _roleManager = new Mock<RoleManager<IdentityRole>>(
                roleStore.Object,
                null,
                null,
                null,
                null);

            _config = new Mock<IConfiguration>();

            _userAccountService = new UserAccountService(
                _userManager.Object,
                _roleManager.Object,
                _config.Object);
        }

        [Fact]
        public async Task DeleteUserAccountAsync_Success()
        {
            var user = new UserApp();
            var identityResult = new IdentityResult();
            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManager.Setup(x => x.DeleteAsync(It.IsAny<UserApp>())).ReturnsAsync(identityResult);

            var result = await _userAccountService.DeleteUserAccountAsync("name");
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteUserAccountAsync_Failed()
        {
            string userName = "name";

            UserApp? user = null;
            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            var result = await _userAccountService.DeleteUserAccountAsync(userName);
            result.Should().Be(new GeneralResponse(false, $"The {userName} wasn't found"));
        }

        [Fact]
        public async Task ChangeRoleAccountAsync_Success()
        {
            RoleRequest request = new RoleRequest
            {
                Role = "User",
                UserName = "Name"
            };
            UserApp userApp = new UserApp();
            IList<string> roles = new List<string>();
            var identityResult = IdentityResult.Success;

            _userManager.Setup(x => x.FindByNameAsync(
                It.IsAny<string>()))
                .ReturnsAsync(userApp);

            _userManager.Setup(x => x.GetRolesAsync(userApp))
                .ReturnsAsync(roles);

            _userManager.Setup(x => x.RemoveFromRoleAsync(userApp, roles.FirstOrDefault()))
                .ReturnsAsync(identityResult);

            _userManager.Setup(x => x.AddToRoleAsync(userApp, request.Role))
                .ReturnsAsync(identityResult);

            var result = await _userAccountService.ChangeRoleAccountAsync(request);

            result.Should().Be(new GeneralResponse(true, $"Role was change in {request.UserName}"));
        }

        [Fact]
        public async Task ChangeRoleAccountAsync_Failed_Role()
        {
            RoleRequest request = new RoleRequest
            {
                Role = "ewqeqw",
                UserName = "Name"
            };

            var result = await _userAccountService.ChangeRoleAccountAsync(request);

            result.Should().Be(new GeneralResponse(false, $"Such role: {request.Role}, doesn't exist"));
        }

        [Fact]
        public async Task ChangeRoleAccountAsync_Failed_NotFoundUser()
        {
            RoleRequest request = new RoleRequest
            {
                Role = "User",
                UserName = null
            };

            UserApp? userApp = null;

            _userManager.Setup(x => x.FindByNameAsync(
               It.IsAny<string>()))
               .ReturnsAsync(userApp);

            var result = await _userAccountService.ChangeRoleAccountAsync(request);

            result.Should().Be(new GeneralResponse(false, "User not found"));
        }

        [Fact]
        public async Task ChangeRoleAccountAsync_Failed_Conflicted()
        {
            RoleRequest request = new RoleRequest
            {
                Role = "User",
                UserName = "user"
            };

            UserApp userApp = new UserApp();
            IList<string> roles = new List<string> { "User" };
            _userManager.Setup(x => x.FindByNameAsync(
               It.IsAny<string>()))
            .ReturnsAsync(userApp);

            _userManager.Setup(x => x.GetRolesAsync(userApp))
                .ReturnsAsync(roles);

            var result = await _userAccountService.ChangeRoleAccountAsync(request);

            result.Should().Be(new GeneralResponse(false, "User already has such a role"));
        }

        [Fact]
        public async Task ChangeRoleAccountAsync_Failed_NotFoundAnyRole()
        {
            RoleRequest request = new RoleRequest
            {
                Role = "User",
                UserName = "Name"
            };
            UserApp userApp = new UserApp();
            IList<string> roles = new List<string>();
            var identityResult = IdentityResult.Failed();

            _userManager.Setup(x => x.FindByNameAsync(
                It.IsAny<string>()))
                .ReturnsAsync(userApp);

            _userManager.Setup(x => x.GetRolesAsync(userApp))
                .ReturnsAsync(roles);

            _userManager.Setup(x => x.RemoveFromRoleAsync(userApp, roles.FirstOrDefault()))
                .ReturnsAsync(identityResult);

            var result = await _userAccountService.ChangeRoleAccountAsync(request);

            result.Should().Be(new GeneralResponse(false, "User hasn't any role"));
        }

        [Fact]
        public async Task ChangeRoleAccountAsync_Failed_WhileAdd()
        {
            RoleRequest request = new RoleRequest
            {
                Role = "User",
                UserName = "Name"
            };

            UserApp userApp = new UserApp();
            IList<string> roles = new List<string>();
            var identityResultDeleteRole = IdentityResult.Success;
            var identityResultAddRole = IdentityResult.Failed();

            _userManager.Setup(x => x.FindByNameAsync(
                It.IsAny<string>()))
                .ReturnsAsync(userApp);

            _userManager.Setup(x => x.GetRolesAsync(userApp))
                .ReturnsAsync(roles);

            _userManager.Setup(x => x.RemoveFromRoleAsync(userApp, roles.FirstOrDefault()))
                .ReturnsAsync(identityResultDeleteRole);

            _userManager.Setup(x => x.AddToRoleAsync(userApp, request.Role))
                .ReturnsAsync(identityResultAddRole);

            var result = await _userAccountService.ChangeRoleAccountAsync(request);

            result.Should().Be(new GeneralResponse(false, "Error occured.. please try again"));
        }
    }
}
