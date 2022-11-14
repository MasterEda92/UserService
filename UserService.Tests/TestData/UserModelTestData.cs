using UserService.Core.Entities;
using UserService.DbAccess.Models;

namespace UserService.Tests.TestData;

public static class UserModelTestData
{
    public static IEnumerable<UserModel> GetUserModelsTestData()
    {
        return new List<UserModel>
        {
            new UserModel
            {
                Id = 1,
                UserName = "TestUser1",
                Email = "test@test.com",
                FirstName = "Max",
                LastName = "Mustermann",
                Password = "sfaoe30409fgjae024123"
            },
            new UserModel
            {
                Id = 2,
                UserName = "TestUser2",
                Email = "test2@test.com",
                FirstName = "Julia",
                LastName = "Mustermann",
                Password = "j309usdifjhaz12hnekadsh21"
            }
        };
    }

    public static int GetExistingUserId() => GetUserModelsTestData().ToList()[0].Id;

    public static int GetNotExistingUserId() => 99;

    public static UserModel GetExistingUserModel () => GetUserModelsTestData().ToList()[0];

    public static string GetNotExistingUserEmail() => "blabla@bla.com";

    public static User GetValidUserForAdd()
    {
        return new User
        {
            Id = 0,
            UserName = "Test3",
            Email = "test3@test.com",
            Password = "aldskfwe900900asdgfase82",
            FirstName = "Jon",
            LastName = "Doe"
        };
    }

    public static User GetNotExistingUserForUpdate()
    {
        return new User
        {
            Id = 10,
            UserName = "BlaBla",
            Email = "bla123@bla.com",
            Password = "asdf123gasd9a0lkfdga",
            FirstName = "Test",
            LastName = "Test"
        };
    }

    public static User GetNotExistingUserForDelete() => GetNotExistingUserForUpdate();
    
    public static User GetValidUserForUpdate()
    {
        return new User
        {
            Id = 1,
            UserName = "TestUser5",
            Email = "test@test.com",
            FirstName = "Maximilian",
            LastName = "Mustermann",
            Password = "sfaoe30409fgjae024123"
        };
    }

    public static User GetValidUserForDelete()
    {
        return new User
        {
            Id = 1,
            UserName = "TestUser1",
            Email = "test@test.com",
            FirstName = "Max",
            LastName = "Mustermann",
            Password = "sfaoe30409fgjae024123"
        };
    }
}