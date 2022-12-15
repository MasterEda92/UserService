using UserService.Core.Entities;

namespace UserService.Core.Interfaces;

public interface ITokenGenerator
{
    public string GenerateToken(User user);
    public int? ValidateToken(string? token);
}