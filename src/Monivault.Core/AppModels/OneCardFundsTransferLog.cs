using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Monivault.AppModels
{
    public class OneCardFundsTransferLog : Entity<long>, IHasCreationTime
    {
        [Required]
        public Guid OneCardFundsTransferLogKey { get; set; }

        public int AccountHolderId { get; set; }
        public virtual AccountHolder AccountHolder { get; set; }

        public long? TransactionLogId { get; set; }
        public virtual TransactionLog TransactionLog { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string AgentTransactionId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [Column(TypeName = "varchar(3)")]
        public string ResultCode { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string ResultDescription { get; set; }

        public string Action { get; set; }

        [Required]
        [Column(TypeName = "varchar(10)")]
        public string AccountNumber { get; set; }

        [Required]
        [Column(TypeName = "varchar(5)")]
        public string BankCode { get; set; }

        [Required]
        public int BankId { get; set; }
        public virtual Bank Bank { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Responsects { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ResponseValue { get; set; }

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}
