using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Core.Entities;
using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.DbAccess.DbContext;
using UserService.DbAccess.Models;

namespace UserService.DbAccess.Services;

public class UserStoreDb : IUserStore
{
    private readonly UserServiceDbContext _context;
    private readonly IMapper _mapper;

    public UserStoreDb(UserServiceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<User> AddUser(User newUser)
    {
        var entry = await _context.AddAsync(_mapper.Map<UserModel>(newUser));
        return _mapper.Map<User>(entry.Entity);

    }

    public Task<User> UpdateUser(User user)
    {
        var updatedUser = _context.Users.Update(_mapper.Map<UserModel>(user)).Entity;
        if (updatedUser is null)
            throw new UserNotFoundException(); // TODO: Prüfen ob hier noch andere Fälle auftreten können
        
        return Task.FromResult(_mapper.Map<User>(updatedUser));
    }

    public async Task<User> DeleteUserWithId(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
            throw new UserNotFoundException();
        
        var delUser = _context.Users.Remove(_mapper.Map<UserModel>(user)).Entity;
        return _mapper.Map<User>(delUser);
    }

    public async Task<int> Save()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserWithId(int id)
    {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == id);
        if (user is null)
            throw new UserNotFoundException();
        
        return _mapper.Map<User>(user);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        IEnumerable<UserModel> users = await _context.Users.ToListAsync();
        return _mapper.Map<IEnumerable<User>>(users);
    }

    public async Task<IEnumerable<User>> GetUsers(Func<User, bool> predicate)
    {
        var pred = _mapper.Map<Func<UserModel, bool>>(predicate);
        IEnumerable<UserModel> users = _context.Users.Where(pred).ToList();
        return await Task.FromResult(_mapper.Map<IEnumerable<User>>(users));
    }
}