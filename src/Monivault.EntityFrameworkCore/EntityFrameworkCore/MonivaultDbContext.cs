using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Monivault.Authorization.Roles;
using Monivault.Authorization.Users;
using Monivault.MultiTenancy;
using Monivault.SignUp;

namespace Monivault.EntityFrameworkCore
{
    public class MonivaultDbContext : AbpZeroDbContext<Tenant, Role, User, MonivaultDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<SignUpVerificationCode> SignUpVerificationCodes { get; set; }
        
        public MonivaultDbContext(DbContextOptions<MonivaultDbContext> options)
            : base(options)
        {
        }
    }
}
