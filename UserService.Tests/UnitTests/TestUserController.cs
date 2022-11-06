using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers;
using UserService.API.DTOs;
using UserService.API.Mappings;
using UserService.Core.Entities;
using UserService.Core.Exceptions;
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
    public async Task GetAllUsersShouldReturn404NotFoundWhenThereAreNoUsers()
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
    public async Task GetAllUsersShouldReturn200OkWhenThereAreUsers()
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
    public async Task GetAllUsersShouldReturnListOfAllUsersWhenThereAreUsers()
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
    public async Task GetAllUsersShouldReturnTheCorrectListOfUsersWhenThereAreUsers()
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

    [Fact]
    public async Task GetUserByIdShouldReturn404WhenThereIsNoUserWithGivenId()
    {
        // Arrange
        int userId = 99;
        var mockUserService = new Mock<IUserService>();
        var testUser = UserTestData.GetTestUser();
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(() => throw new UserNotFoundException());

        var userController = new UserController(mockUserService.Object, _userMapper);
        
        // Act
        var result = await userController.GetUserById(userId);

        // Assert
        result.Result.ShouldBeOfType<NotFoundResult>();
    }
    
    [Fact]
    public async Task GetUserByIdShouldReturn200WhenThereIsAUserWithGivenId()
    {
        // Arrange
        int userId = 1;
        var mockUserService = new Mock<IUserService>();
        var testUser = UserTestData.GetTestUser();
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(testUser));

        var userController = new UserController(mockUserService.Object, _userMapper);
        
        // Act
        var result = await userController.GetUserById(userId);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async Task GetUserByIdShouldReturnTheCorrectUserWhenThereIsAUserWithGivenId()
    {
        // Arrange
        int userId = 1;
        var mockUserService = new Mock<IUserService>();
        var testUser = UserTestData.GetTestUser();
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(testUser));

        var userController = new UserController(mockUserService.Object, _userMapper);
        
        // Act
        var result = await userController.GetUserById(userId);
        var actual = Utility.GetObjectResultContent<UserDto>(result);
        
        // Assert
        actual?.Id.ShouldBe(testUser.Id);
        actual?.UserName.ShouldBe(testUser.UserName);
        actual?.Email.ShouldBe(testUser.Email);
        actual?.Password.ShouldBe(testUser.Password);
        actual?.FirstName.ShouldBe(testUser.FirstName);
        actual?.LastName.ShouldBe(testUser.LastName);
    }
}