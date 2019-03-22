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
        public Guid AccountHolderKey { get; set; }

        public string AccountIdentity { get; private set; } = RandomStringGeneratorUtil.GenerateAccountHolderIdentity();

        public decimal AvailableBalance { get; set; }

        public decimal LedgerBalance { get; set; }

        public long UserId { get; set; }
        
        public virtual User User { get; set; }
        
        
    }
}