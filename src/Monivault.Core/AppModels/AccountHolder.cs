using System;
using Abp.Domain.Entities.Auditing;
using Monivault.Authorization.Users;

namespace Monivault.Models
{
    public class AccountHolder : FullAuditedEntity<long>
    {
        public Guid AccountHolderKey { get; set; }

        public string AccountIdentity { get; set; }

        public decimal AvailableBalance { get; set; }

        public decimal LedgerBalance { get; set; }

        public long UserId { get; set; }
        
        public virtual User User { get; set; }
    }
}