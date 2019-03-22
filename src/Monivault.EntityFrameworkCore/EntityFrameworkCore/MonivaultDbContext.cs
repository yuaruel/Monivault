using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Monivault.AppModels;
using Monivault.Authorization.Roles;
using Monivault.Authorization.Users;
using Monivault.MultiTenancy;

namespace Monivault.EntityFrameworkCore
{
    public class MonivaultDbContext : AbpZeroDbContext<Tenant, Role, User, MonivaultDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<VerificationCode> VerificationCodes { get; set; }
        public virtual DbSet<AccountHolder> AccountHolders { get; set; }
        public virtual DbSet<TransactionLog> TransactionLogs { get; set; }
        public virtual DbSet<OneCardPinRedeemLog> OneCardPinRedeemLogs { get; set; }

        public MonivaultDbContext(DbContextOptions<MonivaultDbContext> options)
            : base(options)
        {
        }
    }
}
