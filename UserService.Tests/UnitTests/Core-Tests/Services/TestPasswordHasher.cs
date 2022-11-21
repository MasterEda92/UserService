using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.Core.Services;

namespace UserService.Tests.UnitTests.Core_Tests.Services;

public class TestPasswordHasher
{
    private readonly IPasswordHasher _passwordHasher;
    private const string Password = "DasIstIrgendeinPa$$w0rT!=?"; 
    public TestPasswordHasher()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void HashPasswordShouldNotReturnTheRealPassword()
    {
        // Arrange
        var password = Password;

        // Act
        var hash = _passwordHasher.HashPassword(password);
        
        // Assert
        hash.ShouldNotBe(password);
    }

    [Fact]
    public void HashPasswordShouldThrowEmptyPasswordExceptionWhenGivenPasswordIsEmpty()
    {
        // Arrange
        var password = "";

        // Act & Assert
        Should.Throw<EmptyPasswordException>(() => _passwordHasher.HashPassword(password));
    }

    [Fact]
    public void HashPasswordShouldReturnAHashWhenGivenAPassword()
    {
        // Arrange
        var password = Password;

        // Act
        var hash = _passwordHasher.HashPassword(password);
        
        // Assert
        hash.ShouldNotBeEmpty();
    }

    [Fact]
    public void VerifyPasswordShouldReturnTrueWhenTheCorrectPasswordIsGiven()
    {
        // Arrange
        var password = Password;

        // Act
        var hash = _passwordHasher.HashPassword(password);
        var valid = _passwordHasher.VerifyPassword(password, hash);
        
        // Assert
        valid.ShouldBeTrue();
    }
    
    [Fact]
    public void VerifyPasswordShouldReturnFalseWhenAWrongPasswordIsGiven()
    {
        // Arrange
        var password = Password;

        // Act
        var hash = _passwordHasher.HashPassword(password);
        var valid = _passwordHasher.VerifyPassword("wrong_password", hash);
        
        // Assert
        valid.ShouldBeFalse();
    }
}