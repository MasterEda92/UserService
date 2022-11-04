using UserService.Core.Entities;

namespace UserService.Core.Interfaces;

public interface IUserService
{
    public IEnumerable<User> GetAllUsers();
    public User GetUserById(int id);
    public User GetUserByEmail(string eMail);
    public User GetUserByUserName(string userName);
}