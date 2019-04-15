using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Monivault.AppModels
{
    public class SavingsInterest : Entity<long>, IHasCreationTime, IHasModificationTime
    {
        [Required]
        public int AccountHolderId { get; set; }

        public virtual AccountHolder AccountHolder { get; set; }

        public decimal InterestPrincipal { get; set; } = decimal.Zero;

        public decimal InterestAccrued { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime EndDate { get; set; }

        //Only used during interest calculation, to signify that a debit transaction occurred during a check on the days transaction
        //for the accountHolder. This is set in the forEach function of the transactionlogs.
        [NotMapped]
        public bool IsTransactionDebit { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastModificationTime { get; set; }
        
        public class StatusTypes
        {
            public const string Running = "Running";
            public const string Completed = "Completed";
        }
    }
}