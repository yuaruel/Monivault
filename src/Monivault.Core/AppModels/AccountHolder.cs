using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Monivault.Authorization.Users;
using Monivault.Utils;

namespace Monivault.AppModels
{
    public class AccountHolder : FullAuditedEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountHolderKey { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "varchar(7)")]
        public string AccountIdentity { get; private set; } = RandomStringGeneratorUtil.GenerateAccountHolderIdentity();

        [Required]
        public decimal AvailableBalance { get; set; }

        [Required]
        public decimal LedgerBalance { get; set; }

        [Required]
        public long UserId { get; set; }
        
        public virtual User User { get; set; }

        public int? BankId { get; set; }

        public virtual Bank Bank { get; set; }
        
        [Column(TypeName = "varchar(12)")]
        public string BankAccountNumber { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string BankAccountName { get; set; }
        
        
    }
}