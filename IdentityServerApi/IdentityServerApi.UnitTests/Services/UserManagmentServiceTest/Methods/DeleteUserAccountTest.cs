using FluentAssertions;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityServerApi.UnitTests.Serivces.UserManagmentServiceTest.Methods
{
    public class DeleteUserAccountTest : UserManagmentServiceBaseTest
    {
        public DeleteUserAccountTest() : base()
        {
        }

        [Fact]
        public async Task DeleteUserAccountAsync_Success()
        {
            var user = new UserApp();
            var identityResult = new IdentityResult();
            UserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            UserManager.Setup(x => x.DeleteAsync(It.IsAny<UserApp>())).ReturnsAsync(identityResult);

            var result = await UserManagmentService.DeleteUserAccountAsync("name");
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteUserAccountAsync_Failed()
        {
            string userName = "name";

            UserApp? user = null;
            UserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            var result = await UserManagmentService.DeleteUserAccountAsync(userName);
            result.Should().Be(new GeneralResponse<string>(false, $"The {userName} wasn't found", null!));
        }
    }
}
