using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Monivault.AppModels
{
    public class TaxPayment : CreationAuditedEntity<long>
    {
        [Required]
        public Guid TaxPaymentKey { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Tin { get; set; }

        [Required]
        [Column(TypeName = " varchar(100)")]
        public string FullName { get; set; }

        public decimal Amount { get; set; }

        public DateTimeOffset TaxPeriod { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ReconcilliationPvNumber { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string EmailAddress { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string PhoneNumber { get; set; }

        public int AccountHolderId { get; set; }

        public virtual AccountHolder AccountHolder { get; set; }

        public int TaxTypeId { get; set; }

        public virtual TaxType TaxType { get; set; }
    }
}
