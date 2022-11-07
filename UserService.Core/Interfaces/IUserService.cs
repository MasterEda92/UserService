using UserService.Core.DTOs;
using UserService.Core.Entities;

namespace UserService.Core.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<User>? GetUserById(int id);
    public Task<User> GetUserByEmail(string eMail);
    public Task<User> GetUserByUserName(string userName);

    public Task<User> RegisterNewUser(RegisterUserDto user);

    public Task<User> UpdateUserWithId(int userId, UpdateUserDto user);

    public Task<bool> DeleteUserWithId(int userId);

    public Task<bool> CheckIfUserWithIdExists(int userId);
}