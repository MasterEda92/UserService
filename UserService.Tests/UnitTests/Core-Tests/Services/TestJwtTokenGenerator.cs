using Microsoft.Extensions.Options;
using UserService.Core.Interfaces;
using UserService.Core.Services;
using UserService.Core.Settings;
using UserService.Tests.TestData;

namespace UserService.Tests.UnitTests.Core_Tests.Services;

public class TestJwtTokenGenerator
{
    private readonly ITokenGenerator _tokenGenerator;
    public TestJwtTokenGenerator()
    {
        var options = Options.Create(new JwtSettings
        {
            Secret = "dasAWEWe0721jJUsekö",
            Audience = "https://stefan.eder.test.com/",
            Issuer = "https://stefan.eder.test.com/",
            ExpiryDays = 7
        });
        _tokenGenerator = new JwtTokenGenerator(options);
    }

    #region GenerateTokenTests

    [Fact]
    public void GenerateTokenShouldReturnATokenWhenAUserIsGiven()
    {
        // Arrange
        var user = UserTestData.GetTestUser();
        
        // Act
        var token = _tokenGenerator.GenerateToken(user);
        
        // Assert
        token.ShouldNotBeEmpty();
    }

    #endregion

    #region ValidateTokenTests

    

    #endregion
}