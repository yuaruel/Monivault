using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Monivault.AppModels;
using Monivault.Authorization.Roles;
using Monivault.Authorization.Users;
using Monivault.MultiTenancy;
using Newtonsoft.Json;

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
        public virtual DbSet<OtpSession> OtpSessions { get; set; }
        public virtual DbSet<SavingsInterest> SavingsInterests { get; set; }
        public virtual DbSet<SavingsInterestDetail> SavingsInterestDetails { get; set; }
        public virtual DbSet<AirtimeNetwork> AirtimeNetworks { get; set; }
        public virtual DbSet<OneCardTopupLog> OneCardTopupLogs { get; set; }
        public virtual DbSet<OneCardFundsTransferLog> OneCardFundsTransferLogs { get; set; }
        public virtual DbSet<TaxProfile> TaxProfiles { get; set; }
        public virtual DbSet<TaxType> TaxTypes { get; set; }
        public virtual DbSet<TaxPayment> TaxPayments { get; set; }
        public virtual DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

        public MonivaultDbContext(DbContextOptions<MonivaultDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OtpSession>().Property(p => p.ActionProperty)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v)
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
        }
    }
}
