using UserService.Contracts.DTOs;

namespace UserService.Core.Interfaces;

public interface IUserUpdateValidator
{
    public bool ValidateUserData(UpdateUserDto user);
}