using System.Linq.Expressions;
using UserService.Core.Entities;

namespace UserService.Core.Interfaces;

public interface IUserStore
{
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<User> GetUserWithId(int id);
    public Task<IEnumerable<User>> GetUsers(Expression<Func<User, bool>> predicate); // TODO: Hier IQueryable statt IEnumerable zurückgeben!
    public Task<User> AddUser(User newUser);
    public Task<User> UpdateUser(User user);
    public Task<User> DeleteUserWithId(int id);
    public Task<int> Save();
}