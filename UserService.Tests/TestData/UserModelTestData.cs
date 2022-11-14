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

}