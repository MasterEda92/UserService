using UserService.Contracts.DTOs;

namespace UserService.Core.Interfaces;

public interface IUserLoginValidator
{
    public bool ValidateUserData(LoginUserDto user);
}