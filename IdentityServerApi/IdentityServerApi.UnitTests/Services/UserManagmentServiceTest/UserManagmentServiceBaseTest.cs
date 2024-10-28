using IdentityServerApi.Host.Data.Entities;
using IdentityServerApi.Host.Services;
using IdentityServerApi.Host.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityServerApi.UnitTests.Serivces.UserManagmentServiceTest;

public class UserManagmentServiceBaseTest
{
    protected readonly IUserManagmentService UserManagmentService;

    protected readonly Mock<UserManager<UserApp>> UserManager;
    public UserManagmentServiceBaseTest()
    {
        var userStore = new Mock<IUserStore<UserApp>>();

        UserManager = new Mock<UserManager<UserApp>>(
            userStore.Object,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);

        UserManagmentService = new UserManagmentService(
            UserManager.Object);
    }
}