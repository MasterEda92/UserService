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

    public UserDbContext CreateContext()
        => new UserDbContext();
}