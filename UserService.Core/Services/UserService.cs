using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Core.Interfaces;
using UserService.Core.Entities;
using UserService.Contracts.DTOs;

namespace UserService.Core.Services;

public class UserService_ : IUserService
{
    private readonly IUserStore _userStore;

    public UserService_(IUserStore userStore)
    {
        _userStore = userStore;    
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userStore.GetAllUsers();
    }

    public Task<User?> GetUserById(int id)
    {
        throw new NotImplementedException ();
    }

    public Task<User?> GetUserByEmail(string eMail)
    {
        throw new NotImplementedException ();
    }

    public Task<User?> GetUserByUserName(string userName)
    {
        throw new NotImplementedException ();
    }

    public Task<User> RegisterUser(RegisterUserDto user)
    {
        throw new NotImplementedException ();
    }

    public Task<string> LoginUser(LoginUserDto loginUser)
    {
        throw new NotImplementedException ();
    }

    public Task<User> UpdateUserWithId(int userId, UpdateUserDto user)
    {
        throw new NotImplementedException ();
    }

    public Task<User> DeleteUserWithId(int userId)
    {
        throw new NotImplementedException ();
    }

    public Task<bool> CheckIfUserWithIdExists(int userId)
    {
        throw new NotImplementedException ();
    }
}
