using Microsoft.EntityFrameworkCore;
using UserService.DbAccess.Models;

namespace UserService.DbAccess.DbContext;

public class UserServiceDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<UserModel> Users { get; set; } = null!;

    private readonly string _dbPath;
    
    public UserServiceDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        _dbPath = System.IO.Path.Join(path, "blogging.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: Konfigurierbar machen!
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
    }
}