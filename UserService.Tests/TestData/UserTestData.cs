using System.Collections.Generic;
using UserService.Core.DTOs;
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
}