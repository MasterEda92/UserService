using System.Linq.Expressions;
using UserService.Core.Entities;

namespace UserService.Infrastructure.Interfaces;

public interface IUserStore
{
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<User> GetUserWithId(int id);
    public Task<IQueryable<User>> GetUsers(Expression<Func<User, bool>> predicate);
    public Task<User> AddUser(User newUser);
    public Task<User> UpdateUser(User user);
    public Task<User> DeleteUserWithId(int id);
    public Task<int> Save();
}