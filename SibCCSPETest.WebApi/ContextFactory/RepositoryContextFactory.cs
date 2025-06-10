using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SibCCSPETest.Data;

namespace SibCCSPETest.WebApi
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<DbDataContext>
    {
        public DbDataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<DbDataContext>()
                .UseNpgsql(configuration.GetConnectionString("PostgreConnection"),
                b => b.MigrationsAssembly("SibCCSPETest.WebApi"));

            return new DbDataContext(builder.Options);
        }
    }
}
