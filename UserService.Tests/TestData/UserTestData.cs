using UserService.Contracts.DTOs;
using UserService.Core.Entities;

namespace UserService.Tests.TestData;

public static class UserTestData
{
    public static IEnumerable<User> GetTestUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                UserName = "TestUser1",
                Email = "test@test.com",
                FirstName = "Max",
                LastName = "Mustermann",
                Password = "P4ssw0rd"
            },
            new User
            {
                Id = 2,
                UserName = "TestUser2",
                Email = "test2@test.com",
                FirstName = "Julia",
                LastName = "Mustermann",
                Password = "Pa$$w0rd"
            }
        };

        return users;
    }

    public static User GetTestUser()
    {
        return new User
        {
            Id = 1,
            UserName = "TestUser1",
            Email = "test@test.com",
            FirstName = "Max",
            LastName = "Mustermann",
            Password = "P4ssw0rd"
        };
    }

    public static RegisterUserDto GetValidUserForRegistration()
    {
        return new RegisterUserDto
        {
            UserName = "TestUser3",
            Email = "test3@test.com",
            Password = "P4ssw0rd",
            FirstName = "Stefan",
            LastName = "Eder"
        };
    }

    public static RegisterUserDto GetInvalidUserForRegistration()
    {
        return new RegisterUserDto
        {
            UserName = "TestUser3",
            Email = "test", // invalid E-Mail
            Password = "P4ssw0rd",
            FirstName = "Stefan",
            LastName = "Eder"
        };
    }

    public static UpdateUserDto GetValidUserForUpdate()
    {
        return new UpdateUserDto
        {
            Email = "test@test.com",
            Password = "P4ssw0rd",
            FirstName = "Stefan",
            LastName = "Eder"
        };
    }
    
    public static UpdateUserDto GetInvalidUserForUpdate()
    {
        return new UpdateUserDto
        {
            Email = "test", // invalid E-Mail
            Password = "P4ssw0rd",
            FirstName = "Stefan",
            LastName = "Eder"
        };
    }
    
    public static LoginUserDto GetValidUserForLogin()
    {
        return new LoginUserDto
        {
            UserName = "test",
            Email = "test@test.com",
            Password = "P4ssw0rd",
        };
    }
    
    public static LoginUserDto GetValidUserForLoginWithWrongPassword()
    {
        return new LoginUserDto
        {
            UserName = "test",
            Email = "test@test.com",
            Password = "abc",
        };
    }
    
    public static LoginUserDto GetInvalidUserForLogin()
    {
        return new LoginUserDto
        {
            UserName = "",
            Email = "",
            Password = "",
        };
    }

    public static LoginUserDto GetNotExistingUserForLogin()
    {
        return new LoginUserDto
        {
            UserName = "Foo",
            Email = "foo@test.com",
            Password = "P4ssw0rd",
        };
    }
}

