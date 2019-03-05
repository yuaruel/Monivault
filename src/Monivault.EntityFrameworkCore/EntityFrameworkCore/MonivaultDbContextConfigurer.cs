using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Monivault.EntityFrameworkCore
{
    public static class MonivaultDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MonivaultDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MonivaultDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
