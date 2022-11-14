using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using UserService.Core.Entities;
using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.DbAccess.DbContext;
using UserService.DbAccess.Models;

namespace UserService.DbAccess.Services;

public class UserStoreEfCore : IUserStore
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public UserStoreEfCore(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        IEnumerable<UserModel> users = await _context.Users.ToListAsync();
        return _mapper.Map<IEnumerable<User>>(users);
    }
    
    public async Task<User> GetUserWithId(int id)
    {
        var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == id);
        if (user is null)
            throw new UserNotFoundException();
        
        return _mapper.Map<User>(user);
    }

    public Task<IQueryable<User>> GetUsers(Expression<Func<User, bool>> predicate)
    {
        var filterDb = _mapper.Map<Expression<Func<UserModel, bool>>>(predicate);
        var query = _context.Users.Where(filterDb).UseAsDataSource(_mapper).For<User>();
        
        return Task.FromResult(query.AsQueryable());
    }

    public async Task<User> AddUser(User newUser)
    {
        var entry = await _context.AddAsync(_mapper.Map<UserModel>(newUser));
        return _mapper.Map<User>(entry.Entity);
    }

    public async Task<User> UpdateUser(User user)
    {
        var userForUpdate = await _context.Users.FindAsync(user.Id);
        if (userForUpdate is null)
            throw new UserNotFoundException(); 
        
        _mapper.Map(user, userForUpdate);
        var updatedUser = _context.Users.Update(userForUpdate).Entity;
        return _mapper.Map<User>(updatedUser);
    }

    public async Task<User> DeleteUserWithId(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
            throw new UserNotFoundException();
        
        var delUser = _context.Users.Remove(user).Entity;
        return _mapper.Map<User>(delUser);
    }

    public async Task<int> Save()
    {
        return await _context.SaveChangesAsync();
    }
}