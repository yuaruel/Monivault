using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Monivault.AppModels
{
    public class TaxProfile : FullAuditedEntity<long>
    {
        [Required]
        public Guid TaxProfileKey { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Tin { get; set; }

        [Required]
        public string FullName { get; set; }

        public string ReconcilliationPvNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [Required]
        public int AccountHolderId { get; set; }

        public virtual AccountHolder AccountHolder { get; set; }
    }
}
