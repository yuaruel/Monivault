using System;
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
        public virtual DbSet<Bank> Banks { get; set; }

        public MonivaultDbContext(DbContextOptions<MonivaultDbContext> options)
            : base(options)
        {
        }

/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bank>(options =>
            {
                options.HasData(
                    new Bank {Id = 1, Name = "Access Bank", OneCardBankCode = "ACBT"},
                    new Bank {Id = 2, Name = "CitiBank", OneCardBankCode = "CitBT"},
                    new Bank {Id = 3, Name = "EcoBank", OneCardBankCode = "ECOB"},
                    new Bank {Id = 4, Name = "Fidelity Bank", OneCardBankCode = "FDBN"},
                    new Bank {Id = 5, Name = "First Bank", OneCardBankCode = "FBNT"},
                    new Bank {Id = 6, Name = "FCMB", OneCardBankCode = "FCMB"},
                    new Bank {Id = 7, Name = "Guaranty Trust Bank", OneCardBankCode = "GTBT"},
                    new Bank {Id = 8, Name = "Heritage Bank", OneCardBankCode = "HBNT"},
                    new Bank {Id = 9, Name = "Keystone Bank", OneCardBankCode = "KBNT"},
                    new Bank {Id = 10, Name = "Skye Bank", OneCardBankCode = "SKYE"},
                    new Bank {Id = 11, Name = "Stanbic IBTC Bank", OneCardBankCode = "SIBTC"},
                    new Bank {Id = 12, Name = "Standard Chartered Bank", OneCardBankCode = "SCBN"},
                    new Bank {Id = 13, Name = "Sterling Bank", OneCardBankCode = "SBNT"},
                    new Bank {Id = 14, Name = "Union Bank", OneCardBankCode = "UBNT"},
                    new Bank {Id = 15, Name = "United Bank for Africa", OneCardBankCode = "UBAT"},
                    new Bank {Id = 16, Name = "Unity Bank", OneCardBankCode = "UNITY"},
                    new Bank {Id = 17, Name = "Wema Bank", OneCardBankCode = "WEMA"},
                    new Bank {Id = 18, Name = "Zenith Bank", OneCardBankCode = "ZBNT"}
                );
            });
        }*/
    }
}
