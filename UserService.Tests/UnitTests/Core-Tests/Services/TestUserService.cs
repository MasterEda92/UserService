using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using UserService.Core.Interfaces;
using UserService.Core.Services;
using Moq;
using UserService.Tests.TestData;

namespace UserService.Tests.UnitTests.Core_Tests.Services;

public class TestUserService
{
    public TestUserService()
    {
        
    }

    [Fact]
    public async Task GetUsersShouldReturnUsersWhenThereAreUsers()
    {
        // Arrange
        var userStore = new Mock<IUserStore>();
        userStore.Setup(store => store.GetAllUsers())
            .Returns(Task.FromResult(UserTestData.GetTestUsers()));

        IUserService service = new UserService_(userStore.Object);

        // Act
        var users = (await service.GetAllUsers()).ToList();

        // Assert
        users.Count.ShouldBeGreaterThan(0);
    }
}