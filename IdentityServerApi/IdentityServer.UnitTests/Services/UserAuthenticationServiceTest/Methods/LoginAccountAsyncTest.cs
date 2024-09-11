using FluentAssertions;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Requests;
using Moq;

namespace IdentityServer.UnitTests.Serivces.UserAuthenticationServiceTest.Methods
{
    public class LoginAccountAsyncTest : UserAuthenticationServiceBaseTest
    {
        public LoginAccountAsyncTest() : base()
        {
        }

        [Fact]
        public async Task LoginAccountAsync_Success()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "test12312@example.com", Password = "User@123" };
            var user = new UserApp { Id = "1", UserName = "testuser", Email = "test12312@example.com", PasswordHash = "User@123" };
            var roles = new List<string> { "User" };

            UserManager
                .Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            UserManager
                .Setup(um => um.CheckPasswordAsync(It.IsAny<UserApp>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            UserManager
                .Setup(um => um.GetRolesAsync(It.IsAny<UserApp>()))
                .ReturnsAsync(roles);

            // Act
            var result = await UserAuthenticationService.LoginAccountAsync(loginRequest);

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be("Login completed");
            result.Flag.Should().BeTrue();
            result.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginAccountAsync_Failed_WhenEmailIsInvalid()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "invalid@example.com", Password = "password" };

            UserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((UserApp)null!);

            // Act
            var result = await UserAuthenticationService.LoginAccountAsync(loginRequest);

            // Assert
            result.AccessToken.Should().BeNull();
            result.Message.Should().Be("Invalid email");
            result.Flag.Should().BeFalse();
        }

        [Fact]
        public async Task LoginAccountAsync_Failed_WhenPasswordIsInvalid()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "test@example.com", Password = "wrongpassword" };
            var user = new UserApp { Id = "1", UserName = "testuser", Email = "test@example.com" };

            UserManager
                .Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            UserManager
                .Setup(um => um.CheckPasswordAsync(It.IsAny<UserApp>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await UserAuthenticationService.LoginAccountAsync(loginRequest);

            // Assert
            result.AccessToken.Should().BeNull();
            result.Message.Should().Be("Invalid password");
            result.Flag.Should().BeFalse();
        }

        [Fact]
        public async Task LoginAccountAsync_Failed_RequestIsEmpty()
        {
            // Arrange
            LoginRequest? loginRequest = null;

            // Act
            var result = await UserAuthenticationService.LoginAccountAsync(loginRequest);

            // Assert
            result.AccessToken.Should().BeNull();
            result.Message.Should().Be("Login request is empty");
            result.Flag.Should().BeFalse();
        }
    }
}
