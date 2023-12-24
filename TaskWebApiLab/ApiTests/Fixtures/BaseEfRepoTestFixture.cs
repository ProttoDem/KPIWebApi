using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskWebApiLab.Auth;
using TaskWebApiLab.UnitOfWork;

namespace ApiTests.Fixtures;

public abstract class GoalEfRepoTestFixture
{
    protected ApplicationDbContext _dbContext;

    protected GoalEfRepoTestFixture()
    {
        var options = CreateNewContextOptions();

        _dbContext = new ApplicationDbContext(options);
    }

    protected static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
    {
        // Create a fresh service provider, and therefore a fresh
        // InMemory database instance.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TaskWebApiLab;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            .BuildServiceProvider();

        // Create a new options instance telling the context to use an
        // InMemory database and the new service provider.
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseApplicationServiceProvider(serviceProvider);
        return builder.Options;
    }

    protected GoalRepository GetRepository()
    {
        return new GoalRepository(_dbContext);
    }
}