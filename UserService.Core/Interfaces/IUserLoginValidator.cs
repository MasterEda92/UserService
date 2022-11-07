using UserService.Core.DTOs;

namespace UserService.Core.Interfaces;

public interface IUserLoginValidator
{
    public bool ValidateUserData(LoginUserDto user);
}