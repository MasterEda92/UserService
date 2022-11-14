using Microsoft.EntityFrameworkCore;
using UserService.DbAccess.DbContext;
using UserService.Tests.TestData;

namespace UserService.Tests.Fixtures;

public class TestUserDbFixture
{
    private static readonly object Lock = new();
    private static bool _databaseInitialized;

    public TestUserDbFixture()
    {
        lock (Lock)
        {
            if (_databaseInitialized) 
                return;
            
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.AddRange(UserModelTestData.GetUserModelsTestData());
                context.SaveChanges();
            }

            _databaseInitialized = true;
        }
    }

    public static UserDbContext CreateContext()
    {
         var path = Path.GetDirectoryName(Path.GetDirectoryName(
             System.IO.Path.GetDirectoryName( 
                 System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase )));
        var dbPath = System.IO.Path.Join(path, "..", "users.db");
        var context = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options);
        
        return context;
    }
}