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
    #region CommonTestDataAndFunctions

    private readonly IMapper _userMapper;
    
    public TestUserController()
    {
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new UserProfile()); });
        _userMapper = mapperConfig.CreateMapper();
    }

    private static IUserRegistrationValidator GetValidUserRegistrationValidator()
    {
        var mockUserValidator = new Mock<IUserRegistrationValidator>();
        mockUserValidator.Setup(validator => validator.ValidateUserData(It.IsAny<RegisterUserDto>()))
            .Returns(true);
        return mockUserValidator.Object;
    }
    
    private static IUserUpdateValidator GetValidUserUpdateValidator()
    {
        var mockUserValidator = new Mock<IUserUpdateValidator>();
        mockUserValidator.Setup(validator => validator.ValidateUserData(It.IsAny<UpdateUserDto>()))
            .Returns(true);
        return mockUserValidator.Object;
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

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());

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

        var userController = new UserController(
            mockUserService.Object,
            _userMapper, 
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());

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

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());

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

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());

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
        const int userId = 99;
        var mockUserService = new Mock<IUserService>();
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(() => throw new UserNotFoundException());

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.GetUserById(userId);

        // Assert
        result.Result.ShouldBeOfType<NotFoundResult>();
    }
    
    [Fact]
    public async Task GetUserByIdShouldReturn200WhenThereIsAUserWithGivenId()
    {
        // Arrange
        const int userId = 1;
        var mockUserService = new Mock<IUserService>();
        var testUser = UserTestData.GetTestUser();
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(testUser));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.GetUserById(userId);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async Task GetUserByIdShouldReturnTheCorrectUserWhenThereIsAUserWithGivenId()
    {
        // Arrange
        const int userId = 1;
        var mockUserService = new Mock<IUserService>();
        var testUser = UserTestData.GetTestUser();
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(testUser));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper, 
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.GetUserById(userId);
        var actual = Utility.GetObjectResultContent(result);
        
        // Assert
        actual?.Id.ShouldBe(testUser.Id);
        actual?.UserName.ShouldBe(testUser.UserName);
        actual?.Email.ShouldBe(testUser.Email);
        actual?.FirstName.ShouldBe(testUser.FirstName);
        actual?.LastName.ShouldBe(testUser.LastName);
    }
    
    #endregion

    #region RegisterUserTests

    [Fact]
    public async Task RegisterUserShouldReturn201WhenRegistrationWasSuccessful()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var registerUser = UserTestData.GetValidUserForRegistration();
        var newUser = _userMapper.Map<User>(registerUser);
        
        mockUserService.Setup(service => service.RegisterNewUser(registerUser))
            .Returns(Task.FromResult(newUser));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.RegisterUser(registerUser);
        
        // Assert
        result.Result.ShouldBeOfType<ObjectResult>();
        ((ObjectResult)result.Result).StatusCode.ShouldBe(201);
    }
    
    [Fact]
    public async Task RegisterUserShouldReturnTheRegisteredUserWhenRegistrationWasSuccessful()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var registerUser = UserTestData.GetValidUserForRegistration();
        var newUser = _userMapper.Map<User>(registerUser);
        
        mockUserService.Setup(service => service.RegisterNewUser(registerUser))
            .Returns(Task.FromResult(newUser));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper, 
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.RegisterUser(registerUser);
        var actual = Utility.GetObjectResultContent(result);
        
        // Assert
        actual?.UserName.ShouldBe(registerUser.UserName);
        actual?.Email.ShouldBe(registerUser.Email);
        actual?.FirstName.ShouldBe(registerUser.FirstName);
        actual?.LastName.ShouldBe(registerUser.LastName);
    }

    [Fact]
    public void RegisterUserShouldThrowUserRegistrationDataInvalidExceptionWhenGivenUserDataIsInvalid()
    {
        // Arrange
        var mockUserValidator = new Mock<IUserRegistrationValidator>();
        var invalidUserData = UserTestData.GetInvalidUserForRegistration();
        mockUserValidator.Setup(validator => validator.ValidateUserData(invalidUserData))
            .Returns(false);
        
        var mockUserService = new Mock<IUserService>();
        mockUserService.Setup(service => service.RegisterNewUser(invalidUserData))
            .Returns(Task.FromResult(new User()));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            mockUserValidator.Object,
            GetValidUserUpdateValidator());
        
        // Act and Assert
        Should.Throw<UserRegistrationDataInvalidException>(userController.RegisterUser(invalidUserData));
    }

    #endregion

    #region DeleteUserTests

    [Fact]
    public async Task DeleteUserShouldReturn200WhenGivenUserWasDeletedSuccessfully()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        const int userId = 1;
        mockUserService.Setup(service => service.DeleteUserWithId(userId))
            .Returns(Task.FromResult(true));
        
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(new User()));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper, 
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.DeleteUser(userId);

        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async Task DeleteUserShouldReturn500WhenGivenUserWasDeletedUnsuccessfully()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        const int userId = 1;
        mockUserService.Setup(service => service.DeleteUserWithId(userId))
            .Returns(Task.FromResult(false));
        
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(new User()));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper, 
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.DeleteUser(userId);

        // Assert
        ((StatusCodeResult)result.Result!).StatusCode.ShouldBe(500);
    }

    [Fact]
    public async Task DeleteUserShouldReturn404WhenGivenUserCanNotBeFound()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        const int userId = 99;
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(() => throw new UserNotFoundException());
        mockUserService.Setup(service => service.DeleteUserWithId(userId))
            .Returns(Task.FromResult(false));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper, 
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.DeleteUser(userId);

        // Assert
        ((StatusCodeResult)result.Result!).StatusCode.ShouldBe(404);
    }

    [Fact]
    public async Task DeleteUserShouldReturnTheDeletedUserWhenGivenUserWasDeletedSuccessfully()
    {
        var mockUserService = new Mock<IUserService>();
        var user = UserTestData.GetTestUser();
        mockUserService.Setup(service => service.DeleteUserWithId(user.Id))
            .Returns(Task.FromResult(true));
        mockUserService.Setup(service => service.GetUserById(user.Id))
            .Returns(Task.FromResult(user));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.DeleteUser(user.Id);
        var actual = Utility.GetObjectResultContent(result);

        // Assert
        actual?.Id.ShouldBe(user.Id);
        actual?.UserName.ShouldBe(user.UserName);
        actual?.Email.ShouldBe(user.Email);
        actual?.FirstName.ShouldBe(user.FirstName);
        actual?.LastName.ShouldBe(user.LastName);
    }

    #endregion

    #region UpdateUserTests

    [Fact]
    public async Task UpdateUserShouldReturn200WhenUserWasUpdatedSuccessfully()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        const int userId = 1;
        var updateUser = UserTestData.GetValidUserForUpdate();
        var newUser = _userMapper.Map<User>(updateUser);
        
        mockUserService.Setup(service => service.UpdateUserWithId(userId, updateUser))
            .Returns(Task.FromResult(newUser));
        
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(newUser));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.UpdateUser(userId, updateUser);
        
        // Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
    }
    
    [Fact]
    public async Task UpdateUserShouldReturnTheUpdatedUserWhenUserWasUpdatedSuccessfully()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        const int userId = 1;
        var updateUser = UserTestData.GetValidUserForUpdate();
        var newUser = _userMapper.Map<User>(updateUser);
        
        mockUserService.Setup(service => service.UpdateUserWithId(userId, updateUser))
            .Returns(Task.FromResult(newUser));
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(newUser));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.UpdateUser(userId, updateUser);
        var actual = Utility.GetObjectResultContent(result);
        
        // Assert
        actual?.Email.ShouldBe(updateUser.Email);
        actual?.FirstName.ShouldBe(updateUser.FirstName);
        actual?.LastName.ShouldBe(updateUser.LastName);
    }
    
    [Fact]
    public async Task UpdateUserShouldReturn404WhenGivenUserCanNotBeFound()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        const int userId = 99;
        var updateUser = UserTestData.GetValidUserForUpdate();
        
        mockUserService.Setup(service => service.UpdateUserWithId(userId, updateUser))
            .Returns(Task.FromResult(new User()));
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(() => throw new UserNotFoundException());

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.UpdateUser(userId, updateUser);
        
        // Assert
        ((StatusCodeResult)result.Result!).StatusCode.ShouldBe(404);
    }

    [Fact]
    public async Task UpdateUserShouldReturn400WhenGivenUserDataIsInvalid()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        const int userId = 1;
        var updateUser = UserTestData.GetInvalidUserForUpdate();
        var newUser = _userMapper.Map<User>(updateUser);
        
        mockUserService.Setup(service => service.UpdateUserWithId(userId, updateUser))
            .Returns(Task.FromResult(new User()));
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(newUser));

        var mockUserUpdateValidator = new Mock<IUserUpdateValidator>();
        mockUserUpdateValidator.Setup(validator => validator.ValidateUserData(updateUser))
            .Returns(false);

        var userController = new UserController(
            mockUserService.Object, 
            _userMapper, 
            GetValidUserRegistrationValidator(), 
            mockUserUpdateValidator.Object);
        
        // Act
        var result = await userController.UpdateUser(userId, updateUser);
        
        // Assert
        ((StatusCodeResult)result.Result!).StatusCode.ShouldBe(400);
    }

    [Fact]
    public async Task UpdateUserShouldReturn500WhenGivenUserCouldNotBeDeleted()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        const int userId = 1;
        var updateUser = UserTestData.GetValidUserForUpdate();
        var newUser = _userMapper.Map<User>(updateUser);
        
        mockUserService.Setup(service => service.UpdateUserWithId(userId, updateUser))
            .Returns(() => throw new UserUpdateFailedException());
        mockUserService.Setup(service => service.GetUserById(userId))
            .Returns(Task.FromResult(newUser));

        var userController = new UserController(
            mockUserService.Object,
            _userMapper,
            GetValidUserRegistrationValidator(),
            GetValidUserUpdateValidator());
        
        // Act
        var result = await userController.UpdateUser(userId, updateUser);
        
        // Assert
        ((StatusCodeResult)result.Result!).StatusCode.ShouldBe(500);
    }
    
    #endregion
}