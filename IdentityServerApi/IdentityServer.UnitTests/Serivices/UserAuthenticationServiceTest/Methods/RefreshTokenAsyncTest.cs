using FluentAssertions;
using IdentityServerApi.Host.Data.Entities;
using Infrastructure.Identity;
using Moq;
using static IdentityServer4.Models.IdentityResources;

namespace IdentityServer.UnitTests.Serivces.UserAuthenticationServiceTest.Methods;

public class RefreshTokenAsync : UserAuthenticationServiceBaseTest
{
    public RefreshTokenAsync() : base()
    {
    }

    [Fact]
    public async Task RefreshTokenAsync_Success()
    {
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

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest");

        result.Should().NotBeNull();
        result.Message.Should().Be("The new access token was created");
        result.Flag.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task RefreshTokenAsync_NotFoundRefreshToken_Failed()
    {
        UserAuthenticationRepository
            .Setup(x => x.GetByRefreshToken(It.IsAny<string>()))
            .ReturnsAsync((RefreshTokenEntity)null!);

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest");

        result.Should().NotBeNull();
        result.Message.Should().Be("Not found any refresh token");
        result.Flag.Should().BeFalse();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task RefreshTokenAsync_PasswordHashIsInvalid_Failed()
    {
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

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest");

        result.Should().NotBeNull();
        result.Message.Should().Be("Password in refresh token is invalid");
        result.Flag.Should().BeFalse();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task RefreshTokenAsync_UserIdIsInvalid_Failed()
    {
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

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest");

        result.Should().NotBeNull();
        result.Message.Should().Be("Refresh token is invalid");
        result.Flag.Should().BeFalse();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task RefreshTokenAsync_RefreshTokenExpired_Failed()
    {
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

        var result = await UserAuthenticationService.RefreshToken("refreshtokentest");

        result.Should().NotBeNull();
        result.Message.Should().Be("You need to log in");
        result.Flag.Should().BeFalse();
        result.Data.Should().BeNull();
    }
}
