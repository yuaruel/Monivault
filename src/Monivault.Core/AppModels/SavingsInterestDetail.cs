using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Monivault.AppModels
{
    public class SavingsInterestDetail : Entity<long>, IHasCreationTime
    {
        [Required]
        public long SavingsInterestId { get; set; }

        public virtual SavingsInterest SavingsInterest { get; set; }

        [Required] public decimal TodayInterest { get; set; } = decimal.Zero;

        public decimal PenaltyCharge { get; set; } = decimal.Zero;

        [Required]
        public decimal AccruedInterestBeforeToday { get; set; }

        public decimal PrincipalBeforeTodayCalculation { get; set; } = decimal.Zero;

        public decimal PrincipalAfterTodayCalculation { get; set; } = decimal.Zero;

        [Required]
        public string InterestType { get; set; }
        
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        
        public class InterestTypes
        {
            public const string SimpleInterest = "SimpleInterest";
            public const string CompoundInterest = "CompoundInterest";
        }
    }
}