using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers;
using UserService.API.DTOs;
using UserService.API.Mappings;
using UserService.Core.Entities;
using UserService.Core.Interfaces;
using UserService.Tests.TestData;
using UserService.Tests.Utils;

namespace UserService.Tests.UnitTests;

public class TestUserController
{
    private readonly IMapper _userMapper;

    public TestUserController()
    {
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new UserProfile()); });
        _userMapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task ShouldReturn404NotFoundWhenThereAreNoUsers()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        IEnumerable<User> emptyUsersList = new List<User>();
        mockUserService.Setup(service => service.GetAllUsers())
            .Returns(Task.FromResult(emptyUsersList));

        var userController = new UserController(mockUserService.Object, _userMapper);

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

        var userController = new UserController(mockUserService.Object, _userMapper);

        // Act
        var result = await userController.GetAllUsers();

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ShouldReturnListOfAllUsersWhenThereAreUsers()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var allUsers = UserTestData.GetTestUsers();
        mockUserService.Setup(service => service.GetAllUsers())
            .Returns(Task.FromResult(allUsers));

        var userController = new UserController(mockUserService.Object, _userMapper);

        // Act
        var result = await userController.GetAllUsers();
        //var actual = (result.Result as OkObjectResult).Value as IEnumerable<UserDto>;
        var actual = Utility.GetObjectResultContent<IEnumerable<UserDto>>(result!);

        // Assert
        actual.ShouldNotBeNull();
        actual!.Count().ShouldBe(allUsers.Count());
    }

    [Fact]
    public async Task ShouldReturnTheCorrectListOfUsersWhenThereAreUsers()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var allUsers = UserTestData.GetTestUsers();
        mockUserService.Setup(service => service.GetAllUsers())
            .Returns(Task.FromResult(allUsers));

        var userController = new UserController(mockUserService.Object, _userMapper);

        // Act
        var result = await userController.GetAllUsers();
        //var actual = (result.Result as OkObjectResult).Value as IEnumerable<UserDto>;
        var actual = Utility.GetObjectResultContent<IEnumerable<UserDto>>(result!);

        // Assert
        foreach (var (item, index) in allUsers.WithIndex())
        {
            var compareUser = (actual!.ToList())[index];
            item.Id.ShouldBe(compareUser.Id);
            item.UserName.ShouldBe(compareUser.UserName);
            item.Email.ShouldBe(compareUser.Email);
            item.Password.ShouldBe(compareUser.Password);
            item.FirstName.ShouldBe(compareUser.FirstName);
            item.LastName.ShouldBe(compareUser.LastName);
        }
    }
}