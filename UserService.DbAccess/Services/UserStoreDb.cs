using UserService.Core.Entities;
using UserService.Core.Interfaces;

namespace UserService.DbAccess.Services;

public class UserStoreDb : IUserStore
{
    public Task<User> AddUser(User newUser)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteUserWithId(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserWithId(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllUsers()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetUsers(Func<User, bool> predicate)
    {
        throw new NotImplementedException();
    }
}