using FluentAssertions;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityServer.UnitTests.Serivces.UserAccountServiceTest.Methods
{
    public class DeleteUserAccountTest : UserAccountServiceBaseTest
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

            var result = await UserAccountService.DeleteUserAccountAsync("name");
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteUserAccountAsync_Failed()
        {
            string userName = "name";

            UserApp? user = null;
            UserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            var result = await UserAccountService.DeleteUserAccountAsync(userName);
            result.Should().Be(new GeneralResponse(false, $"The {userName} wasn't found"));
        }
    }
}
