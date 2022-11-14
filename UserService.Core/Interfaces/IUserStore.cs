using UserService.Core.Entities;

namespace UserService.Core.Interfaces;

public interface IUserStore
{
    public Task<User> AddUser(User newUser);
    public Task<User> UpdateUser(User user);
    public Task<User> DeleteUserWithId(int id);
    public Task<int> Save();
    public Task<User> GetUserWithId(int id);
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<IEnumerable<User>> GetUsers(Func<User, bool> predicate);
}