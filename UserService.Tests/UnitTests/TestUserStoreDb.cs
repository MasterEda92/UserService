using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.DbAccess.Mappings;
using UserService.DbAccess.Services;
using UserService.Tests.Fixtures;
using UserService.Tests.TestData;
using UserService.Tests.Utils;

namespace UserService.Tests.UnitTests;

public class TestUserStoreDbRead : IClassFixture<TestUserDbFixture>
{
    #region ctor and fields

    private readonly TestUserDbFixture _fixture;
    private readonly IMapper _userMapper;

    public TestUserStoreDbRead(TestUserDbFixture fixture)
    {
        _fixture = fixture;
        
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddExpressionMapping();
            mc.AddProfile(new UserModelProfile());
        });
        _userMapper = mapperConfig.CreateMapper();
    }

    #endregion

    #region tests

    #region GetAllUsersTests

    [Fact]
    public async Task GetAllUsersShouldReturnAllUsersWhenThereAreUsersInTheDb()
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        IUserStore store = new UserStoreDb(context, _userMapper);
        
        // Act
        var users = await store.GetAllUsers();

        // Assert
        users.Count().ShouldBe(UserModelTestData.GetUserModelsTestData().Count());
    }

    [Fact]
    public async Task GetAllUsersShouldReturnTheCorrectUsersWhenThereAreUsersInTheDb()
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        IUserStore store = new UserStoreDb(context, _userMapper);
        var allUsers = UserModelTestData.GetUserModelsTestData().ToList();
        
        // Act
        var users = (await store.GetAllUsers()).ToList();

        // Assert
        foreach (var (item, index) in allUsers.WithIndex())
        {
            var compareUser = users[index];
            item.Id.ShouldBe(compareUser.Id);
            item.UserName.ShouldBe(compareUser.UserName);
            item.Email.ShouldBe(compareUser.Email);
            item.Password.ShouldBe(compareUser.Password);
            item.FirstName.ShouldBe(compareUser.FirstName);
            item.LastName.ShouldBe(compareUser.LastName);
        }
    }

    #endregion

    #region GetUserWithIdTest

    [Fact]
    public async Task GetUserWithIdShouldReturnAUserWhenUserWithGivenIdExists()
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        IUserStore store = new UserStoreDb(context, _userMapper);
        
        // Act
        var user = await store.GetUserWithId(UserModelTestData.GetExistingUserId());
        
        // Assert
        user.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetUserWithIdShouldReturnTheCorrectUserWhenUserWithGivenIdExists()
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        IUserStore store = new UserStoreDb(context, _userMapper);
        var testUser = UserModelTestData.GetExistingUserModel();
        
        // Act
        var user = await store.GetUserWithId(testUser.Id);
        
        // Assert
        user.Id.ShouldBe(testUser.Id);
        user.UserName.ShouldBe(testUser.UserName);
        user.Email.ShouldBe(testUser.Email);
        user.FirstName.ShouldBe(testUser.FirstName);
        user.LastName.ShouldBe(testUser.LastName);
        user.Password.ShouldBe(testUser.Password);
    }

    [Fact]
    public async Task GetUserWithIdShouldThrowUserNotFoundExceptionWhenUserWithGivenIdDoesNotExist()
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        IUserStore store = new UserStoreDb(context, _userMapper);
        
        // Act and Assert
        await Should.ThrowAsync<UserNotFoundException>(store.GetUserWithId(UserModelTestData.GetNotExistingUserId()));
    }

    #endregion

    #region GetUsersTests

    [Fact]
    public async Task GetUsersShouldReturnTheCorrectAmountOfUsersWhenUsersWithGivenPredicateExist()
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        IUserStore store = new UserStoreDb(context, _userMapper);
        var testUser = UserModelTestData.GetExistingUserModel();
        
        // Act
        var users = await store.GetUsers(user => user.Email == testUser.Email);

        // Assert
        users.Count().ShouldBe(1);
    }
    
    [Fact]
    public async Task GetUsersShouldReturnZeroUsersWhenUsersWithGivenPredicateDoNotExist()
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        IUserStore store = new UserStoreDb(context, _userMapper);
        
        // Act
        var users = await store.GetUsers(user => user.Email == UserModelTestData.GetNotExistingUserEmail());

        // Assert
        users.Count().ShouldBe(0);
    }
    
    [Fact]
    public async Task GetUsersShouldReturnTheCorrectUsersWhenUsersWithGivenPredicateExist()
    {
        // Arrange
        await using var context = _fixture.CreateContext();
        IUserStore store = new UserStoreDb(context, _userMapper);
        var testUser = UserModelTestData.GetExistingUserModel();
        
        // Act
        var users = (await store.GetUsers(user => user.Email == testUser.Email)).ToList();

        // Assert
        users[0].Id.ShouldBe(testUser.Id);
        users[0].UserName.ShouldBe(testUser.UserName);
        users[0].Email.ShouldBe(testUser.Email);
        users[0].Password.ShouldBe(testUser.Password);
        users[0].FirstName.ShouldBe(testUser.FirstName);
        users[0].LastName.ShouldBe(testUser.LastName);
    }

    #endregion

    #endregion

    #region private helper functions



    #endregion
}

public class TestUserStoreDbWrite : IClassFixture<TestUserDbFixture>
{
    #region ctor and fields

    private readonly TestUserDbFixture _fixture;
    private readonly IMapper _userMapper;

    public TestUserStoreDbWrite(TestUserDbFixture fixture)
    {
        _fixture = fixture;
        
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new UserModelProfile()); });
        _userMapper = mapperConfig.CreateMapper();
    }

    #endregion

    #region tests

    
    
    #endregion

    #region private helper functions

    

    #endregion
}