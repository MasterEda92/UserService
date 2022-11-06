using UserService.Core.DTOs;

namespace UserService.Core.Interfaces;

public interface IUserValidator
{
    public bool ValidateUserData(RegisterUserDto user);
}