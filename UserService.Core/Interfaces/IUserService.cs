using UserService.Core.Entities;

namespace UserService.Core.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<User> GetUserById(int id);
    public Task<User> GetUserByEmail(string eMail);
    public Task<User> GetUserByUserName(string userName);
}