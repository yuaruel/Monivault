using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Monivault.Configuration;
using Monivault.Web;

namespace Monivault.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class MonivaultDbContextFactory : IDesignTimeDbContextFactory<MonivaultDbContext>
    {
        public MonivaultDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MonivaultDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            MonivaultDbContextConfigurer.Configure(builder, configuration.GetConnectionString(MonivaultConsts.ConnectionStringName));

            return new MonivaultDbContext(builder.Options);
        }
    }
}
