﻿using FluentAssertions;
using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityServerApi.UnitTests.Serivces.UserManagmentServiceTest.Methods
{
    public class CreateUserAccountAsyncTest : UserManagmentServiceBaseTest
    {
        public CreateUserAccountAsyncTest() : base()
        {
        }

        [Fact]
        public async Task CreateUserAccountAsync_Success()
        {
            UserRequest request = new UserRequest
            {
                Name = "Test",
                Password = "User@123",
                ConfirmPassword = "User@123",
                Email = "myacc@gmail.com"
            };

            UserApp userApp = new UserApp
            {
                UserName = request.Name,
                Email = request.Email,
                PasswordHash = request.Password,
            };
            var identityResult = IdentityResult.Success;

            UserManager.Setup(x => x.FindByNameAsync(request.Name))
                .ReturnsAsync((UserApp)null);

            UserManager.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((UserApp)null);

            UserManager.Setup(x => x.CreateAsync(It.IsAny<UserApp>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            UserManager.Setup(x => x.AddToRoleAsync(It.IsAny<UserApp>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await UserManagmentService.CreateUserAccountAsync(request);

            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Account Created");
        }

        [Fact]
        public async Task CreateUserAccountAsync_Failed_RequestIsEmpty()
        {
            UserRequest? request = null;

            var result = await UserManagmentService.CreateUserAccountAsync(request);

            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Model is empty");
        }

        [Fact]
        public async Task CreateUserAccountAsync_Failed_UserNameRegisteredAlready()
        {
            UserRequest request = new UserRequest
            {
                Name = "Test",
                Password = "User@123",
                ConfirmPassword = "User@123",
                Email = "myacc@gmail.com"
            };

            UserApp userApp = new UserApp
            {
                UserName = "Test"
            };
            var identityResult = IdentityResult.Success;

            UserManager.Setup(x => x.FindByNameAsync(request.Name))
                .ReturnsAsync(userApp);

            var result = await UserManagmentService.CreateUserAccountAsync(request);

            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be($"User: {userApp.UserName} registered already");
        }

        [Fact]
        public async Task CreateUserAccountAsync_Failed_UserEmailRegisteredAlready()
        {
            UserRequest request = new UserRequest
            {
                Name = "Test",
                Password = "User@123",
                ConfirmPassword = "User@123",
                Email = "myacc@gmail.com"
            };

            UserApp userApp = new UserApp
            {
                UserName = request.Name,
                Email = request.Email,
                PasswordHash = request.Password,
            };

            var identityResult = IdentityResult.Success;

            UserManager.Setup(x => x.FindByNameAsync(request.Name))
            .ReturnsAsync(userApp);

            UserManager.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(userApp);

            var result = await UserManagmentService.CreateUserAccountAsync(request);

            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be($"User: {userApp.UserName} registered already");
        }

        [Fact]
        public async Task CreateUserAccountAsync_Failed_SomeProblemWithCreating()
        {
            UserRequest request = new UserRequest
            {
                Name = "Test",
                Password = "User@123",
                ConfirmPassword = "User@123",
                Email = "myacc@gmail.com"
            };

            UserApp userApp = new UserApp
            {
                UserName = request.Name,
                Email = request.Email,
                PasswordHash = request.Password,
            };
            var identityResult = IdentityResult.Success;

            UserManager.Setup(x => x.FindByNameAsync(request.Name))
            .ReturnsAsync((UserApp)null);

            UserManager.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((UserApp)null);

            UserManager.Setup(x => x.CreateAsync(It.IsAny<UserApp>(), request.Password))
                .ReturnsAsync(IdentityResult.Failed());

            var result = await UserManagmentService.CreateUserAccountAsync(request);

            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Error occured.. please try again");
        }
    }
}
