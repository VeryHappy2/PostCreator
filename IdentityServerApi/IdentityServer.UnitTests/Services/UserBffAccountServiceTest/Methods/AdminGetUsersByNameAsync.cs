using FluentAssertions;
using IdentityServerApi.Host.Models.Responses;
using Moq;

namespace IdentityServer.UnitTests.Serivces.UserBffAccountServiceTest.Methods
{
    public class AdminGetUsersByNameAsync : UserBffAccountServiceBaseTest
    {
        public AdminGetUsersByNameAsync() : base()
        {
        }

        [Fact]
        public async Task AdminGetUsersByNameAsync_Success()
        {
            // Arrange
            var userName = "testuser";
            var expectedUsers = new List<SearchAdminUserResponse>
            {
                new SearchAdminUserResponse { UserName = "testuser1", RoleName = "User" },
                new SearchAdminUserResponse { UserName = "testuser2", RoleName = "User" }
            };

            _userBffAccountRepositoryMock
                .Setup(repo => repo.AdminGetUsersByNameAsync(userName))
                .ReturnsAsync(new GeneralResponse<List<SearchAdminUserResponse>>(true, "Successfully", expectedUsers));

            // Act
            var result = await _userBffAccountService.AdminGetUsersByNameAsync(userName);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Data.Should().BeSameAs(expectedUsers);
            result.Data.Count.Should().Be(expectedUsers.Count);
        }

        [Fact]
        public async Task AdminGetUsersByNameAsync_Failed()
        {
            // Arrange
            var userName = "testuser";
            SearchAdminUserResponse? expectedUsers = null;

            _userBffAccountRepositoryMock
                .Setup(repo => repo.AdminGetUsersByNameAsync(userName))
                .ReturnsAsync(new GeneralResponse<List<SearchAdminUserResponse>>(false, $"Not found any users by {userName}", null!));

            // Act
            var result = await _userBffAccountService.AdminGetUsersByNameAsync(userName);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be($"Not found any users by {userName}");
            result.Data.Should().BeNull();
        }
    }
}
