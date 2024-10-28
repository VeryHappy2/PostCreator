using FluentAssertions;
using IdentityServerApi.Host.Data.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Moq;

namespace IdentityServerApi.UnitTests.Serivces.UserAuthenticationServiceTest.Methods;

public class RefreshTokenAsync : UserAuthenticationServiceBaseTest
{
    public RefreshTokenAsync() : base()
    {
    }

    [Fact]
    public async Task RefreshTokenAsync_Success()
    {
        var context = new DefaultHttpContext();
        UserAuthenticationRepository
            .Setup(x => x.GetByRefreshToken(It.IsAny<string>()))
            .ReturnsAsync(new RefreshTokenEntity
            {
                Id = 21,
                UserId = "1",
                RefreshToken = "refreshtokentest",
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(10),
                UserPasswordHash = "hash"
            });

        UserManager
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserApp
            {
                Id = "1",
                Email = "myacc@gmail.com",
                PasswordHash = "hash",
                UserName = "name"
            });

        UserManager
            .Setup(x => x.GetRolesAsync(It.IsAny<UserApp>()))
            .ReturnsAsync(new List<string>
            {
                AuthRoles.User
            });

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest", context);

        result.Should().NotBeNull();
        result.Message.Should().Be("The new access token was created");
        result.Flag.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task RefreshTokenAsync_NotFoundRefreshToken_Failed()
    {
        var context = new DefaultHttpContext();
        UserAuthenticationRepository
            .Setup(x => x.GetByRefreshToken(It.IsAny<string>()))
            .ReturnsAsync((RefreshTokenEntity)null!);

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest", context);

        result.Should().NotBeNull();
        result.Message.Should().Be("You need to log in again");
        result.Flag.Should().BeFalse();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task RefreshTokenAsync_PasswordHashIsInvalid_Failed()
    {
        var context = new DefaultHttpContext();
        UserAuthenticationRepository
            .Setup(x => x.GetByRefreshToken(It.IsAny<string>()))
            .ReturnsAsync(new RefreshTokenEntity
            {
                Id = 21,
                UserId = "1",
                RefreshToken = "refreshtokentest",
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(10),
                UserPasswordHash = "wqeqwe"
            });

        UserManager
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserApp
            {
                Id = "1",
                Email = "myacc@gmail.com",
                PasswordHash = "hash",
                UserName = "name"
            });

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest", context);

        result.Should().NotBeNull();
        result.Message.Should().Be("Password in refresh token is invalid");
        result.Flag.Should().BeFalse();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task RefreshTokenAsync_UserIdIsInvalid_Failed()
    {
        var context = new DefaultHttpContext();

        UserAuthenticationRepository
            .Setup(x => x.GetByRefreshToken(It.IsAny<string>()))
            .ReturnsAsync(new RefreshTokenEntity
            {
                Id = 21,
                UserId = "1",
                RefreshToken = "refreshtokentest",
                RefreshTokenExpiry = DateTime.UtcNow.AddDays(10),
                UserPasswordHash = "wqeqwe"
            });

        UserManager
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((UserApp)null!);

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest", context);

        result.Should().NotBeNull();
        result.Message.Should().Be("Refresh token is invalid");
        result.Flag.Should().BeFalse();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task RefreshTokenAsync_RefreshTokenExpired_Failed()
    {
        var context = new DefaultHttpContext();

        UserAuthenticationRepository
            .Setup(x => x.GetByRefreshToken(It.IsAny<string>()))
            .ReturnsAsync(new RefreshTokenEntity
            {
                Id = 21,
                UserId = "1",
                RefreshToken = "refreshtokentest",
                RefreshTokenExpiry = DateTime.UtcNow,
                UserPasswordHash = "hash"
            });

        UserManager
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new UserApp
            {
                Id = "1",
                Email = "myacc@gmail.com",
                PasswordHash = "hash",
                UserName = "name"
            });

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest", context);

        result.Should().NotBeNull();
        result.Message.Should().Be("You need to log in");
        result.Flag.Should().BeFalse();
        result.Data.Should().BeNull();
    }
}
