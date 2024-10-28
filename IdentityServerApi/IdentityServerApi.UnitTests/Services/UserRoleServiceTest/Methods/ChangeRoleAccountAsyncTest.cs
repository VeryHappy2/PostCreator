using FluentAssertions;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Requests;
using IdentityServerApi.Host.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityServerApi.UnitTests.Serivces.UserRoleServiceTest.Methods
{
    public class ChangeRoleAccountAsyncTest : UserRoleServiceBaseTest
    {
        public ChangeRoleAccountAsyncTest() : base()
        {
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

            UserManager.Setup(x => x.FindByNameAsync(
                It.IsAny<string>()))
                .ReturnsAsync(userApp);

            UserManager.Setup(x => x.GetRolesAsync(userApp))
                .ReturnsAsync(roles);

            UserManager.Setup(x => x.RemoveFromRoleAsync(userApp, roles.FirstOrDefault()))
                .ReturnsAsync(identityResult);

            UserManager.Setup(x => x.AddToRoleAsync(userApp, request.Role))
                .ReturnsAsync(identityResult);

            var result = await UserRoleService.ChangeRoleAccountAsync(request);

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

            var result = await UserRoleService.ChangeRoleAccountAsync(request);

            result.Should().Be(new GeneralResponse(false, $"Such role: {request.Role}, doesn't exist"));
        }

        [Fact]
        public async Task ChangeRoleAccountAsync_Failed_NotFoundUser()
        {
            RoleRequest request = new RoleRequest
            {
                Role = "User",
                UserName = null!
            };

            UserApp? userApp = null;

            UserManager.Setup(x => x.FindByNameAsync(
               It.IsAny<string>()))
               .ReturnsAsync(userApp);

            var result = await UserRoleService.ChangeRoleAccountAsync(request);

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
            UserManager.Setup(x => x.FindByNameAsync(
               It.IsAny<string>()))
            .ReturnsAsync(userApp);

            UserManager.Setup(x => x.GetRolesAsync(userApp))
                .ReturnsAsync(roles);

            var result = await UserRoleService.ChangeRoleAccountAsync(request);

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

            UserManager.Setup(x => x.FindByNameAsync(
                It.IsAny<string>()))
                .ReturnsAsync(userApp);

            UserManager.Setup(x => x.GetRolesAsync(userApp))
                .ReturnsAsync(roles);

            UserManager.Setup(x => x.RemoveFromRoleAsync(userApp, roles.FirstOrDefault()))
                .ReturnsAsync(identityResult);

            var result = await UserRoleService.ChangeRoleAccountAsync(request);

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

            UserManager.Setup(x => x.FindByNameAsync(
                It.IsAny<string>()))
                .ReturnsAsync(userApp);

            UserManager.Setup(x => x.GetRolesAsync(userApp))
                .ReturnsAsync(roles);

            UserManager.Setup(x => x.RemoveFromRoleAsync(userApp, roles.FirstOrDefault()))
                .ReturnsAsync(identityResultDeleteRole);

            UserManager.Setup(x => x.AddToRoleAsync(userApp, request.Role))
                .ReturnsAsync(identityResultAddRole);

            var result = await UserRoleService.ChangeRoleAccountAsync(request);

            result.Should().Be(new GeneralResponse(false, "Error occured.. please try again"));
        }
    }
}
