using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers;
using UserService.API.DTOs;
using UserService.Core.Entities;
using UserService.Core.Interfaces;
using UserService.Tests.TestData;

namespace UserService.Tests.UnitTests;

public class TestUserController
{
    [Fact]
    public async Task ShouldReturn404NotFoundWhenThereAreNoUsers()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        IEnumerable<User> emptyUsersList = new List<User>();
        mockUserService.Setup(service => service.GetAllUsers())
            .Returns(Task.FromResult(emptyUsersList));

        var userController = new UserController(mockUserService.Object);

        // Act
        var result = await userController.GetAllUsers();

        // Assert
        result.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task ShouldReturn200OkWhenThereAreUsers()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService.Setup(service => service.GetAllUsers())
            .Returns(Task.FromResult(UserTestData.GetTestUsers()));

        var userController = new UserController(mockUserService.Object);

        // Act
        var result = await userController.GetAllUsers();

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ShouldReturnListOfUsersWhenThereAreUsers()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var allUsers = UserTestData.GetTestUsers();
        mockUserService.Setup(service => service.GetAllUsers())
            .Returns(Task.FromResult(allUsers));

        var userController = new UserController(mockUserService.Object);

        // Act
        var result = await userController.GetAllUsers();
        var actual = (result.Result as OkObjectResult).Value as IEnumerable<UserDto>;

        // Assert
        actual.Count().ShouldBe(allUsers.Count());
    }
}