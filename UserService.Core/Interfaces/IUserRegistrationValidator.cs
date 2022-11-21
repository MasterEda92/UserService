using UserService.Contracts.DTOs;

namespace UserService.Core.Interfaces;

public interface IUserRegistrationValidator
{
    public bool ValidateUserData(RegisterUserDto user);
}