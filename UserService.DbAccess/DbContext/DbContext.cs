using Microsoft.EntityFrameworkCore;
using UserService.DbAccess.Models;

namespace UserService.DbAccess.DbContext;

public class UserDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<UserModel> Users { get; set; } = null!;
    
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }
}