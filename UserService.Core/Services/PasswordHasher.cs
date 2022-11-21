using UserService.Core.Exceptions;
using UserService.Core.Interfaces;

namespace UserService.Core.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new EmptyPasswordException();
        
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}