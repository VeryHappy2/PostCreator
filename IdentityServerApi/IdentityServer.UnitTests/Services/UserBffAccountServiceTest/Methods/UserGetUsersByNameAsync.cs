using FluentAssertions;
using IdentityServerApi.Host.Models.Responses;
using Moq;

namespace IdentityServer.UnitTests.Serivces.UserBffAccountServiceTest.Methods
{
    public class UserGetUsersByNameAsync : UserBffAccountServiceBaseTest
    {
        public UserGetUsersByNameAsync() : base()
        {
        }

        [Fact]
        public async Task UserGetUsersByNameAsync_Success()
        {
            // Arrange
            var userName = "testuser";
            var expectedUsers = new List<SearchUserResponse>
            {
                new SearchUserResponse { UserName = "testuser1", Id = "1" },
                new SearchUserResponse { UserName = "testuser2", Id = "2" }
            };

            _userBffAccountRepositoryMock
                .Setup(x => x.UserGetUsersByNameAsync(userName))
                .ReturnsAsync(new GeneralResponse<List<SearchUserResponse>>(true, "Successfully", expectedUsers));

            // Act
            var result = await _userBffAccountService.UserGetUsersByNameAsync(userName);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Data.Should().BeSameAs(expectedUsers);
            result.Data.Count.Should().Be(expectedUsers.Count);
        }

        [Fact]
        public async Task UserGetUsersByNameAsync_Failed()
        {
            // Arrange
            var userName = "testuser";
            SearchUserResponse? expectedUsers = null;

            _userBffAccountRepositoryMock
                .Setup(repo => repo.UserGetUsersByNameAsync(userName))
                .ReturnsAsync(new GeneralResponse<List<SearchUserResponse>>(false, $"Not found any users by {userName}", null!));

            // Act
            var result = await _userBffAccountService.UserGetUsersByNameAsync(userName);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be($"Not found any users by {userName}");
            result.Data.Should().BeNull();
        }
    }
}
