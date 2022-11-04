using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers;
using UserService.Core.Entities;
using UserService.Core.Interfaces;

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
}