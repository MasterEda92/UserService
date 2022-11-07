using UserService.Core.DTOs;

namespace UserService.Core.Interfaces;

public interface IUserUpdateValidator
{
    public bool ValidateUserData(UpdateUserDto user);
}