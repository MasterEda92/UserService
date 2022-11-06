using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.API.Controllers;
using UserService.Core.DTOs;
using UserService.Core.Entities;
using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.Core.Mappings;
using UserService.Tests.TestData;
using UserService.Tests.Utils;

namespace UserService.Tests.UnitTests;

public class TestUserController
{
    #region privateFieldsAndCtor

    private readonly IMapper _userMapper;

    public TestUserController()
    {
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new UserProfile()); });
        _userMapper = mapperConfig.CreateMapper();
    }

    #endregion

    #region GetAllUsersTests

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
        var allUsers = UserTestData.GetTestUsers().ToList();
        mockUserService.Setup(service => service.GetAllUsers())
            .Returns(Task.FromResult((IEnumerable<User>)allUsers));

        var userController = new UserController(mockUserService.Object, _userMapper);

        // Act
        var result = await userController.GetAllUsers();
        var actual = Utility.GetObjectResultContent<IEnumerable<UserDto>>(result!)!.ToList();

        // Assert
        actual.ShouldNotBeNull();
        actual.Count.ShouldBe(allUsers.Count);
    }

    [Fact]
    public async Task GetAllUsersShouldReturnTheCorrectListOfUsersWhenThereAreUsers()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var allUsers = UserTestData.GetTestUsers().ToList();
        mockUserService.Setup(service => service.GetAllUsers())
            .Returns(Task.FromResult((IEnumerable<User>)allUsers));

        var userController = new UserController(mockUserService.Object, _userMapper);

        // Act
        var result = await userController.GetAllUsers();
        var actual = Utility.GetObjectResultContent<IEnumerable<UserDto>>(result!)!.ToList();

        // Assert
        foreach (var (item, index) in allUsers.WithIndex())
        {
            var compareUser = actual[index];
            item.Id.ShouldBe(compareUser.Id);
            item.UserName.ShouldBe(compareUser.UserName);
            item.Email.ShouldBe(compareUser.Email);
            item.Password.ShouldBe(compareUser.Password);
            item.FirstName.ShouldBe(compareUser.FirstName);
            item.LastName.ShouldBe(compareUser.LastName);
        }
    }
    
    #endregion

    #region GetUserByIdTests

    [Fact]
    public async Task GetUserByIdShouldReturn404WhenThereIsNoUserWithGivenId()
    {
        // Arrange
        int userId = 99;
        var mockUserService = new Mock<IUserService>();
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
        var actual = Utility.GetObjectResultContent(result);
        
        // Assert
        actual?.Id.ShouldBe(testUser.Id);
        actual?.UserName.ShouldBe(testUser.UserName);
        actual?.Email.ShouldBe(testUser.Email);
        actual?.Password.ShouldBe(testUser.Password);
        actual?.FirstName.ShouldBe(testUser.FirstName);
        actual?.LastName.ShouldBe(testUser.LastName);
    }
    
    #endregion

    #region RegisterUserTests

    [Fact]
    public async Task RegisterUserShouldReturn200WhenRegistrationWasSuccessful()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var registerUser = UserTestData.GetValidUserForRegistration();
        var newUser = _userMapper.Map<User>(registerUser);
        
        mockUserService.Setup(service => service.RegisterNewUser(registerUser))
            .Returns(Task.FromResult(newUser));

        var userController = new UserController(mockUserService.Object, _userMapper);
        
        // Act
        var result = await userController.RegisterUser(registerUser);
        
        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
    }

    #endregion
}