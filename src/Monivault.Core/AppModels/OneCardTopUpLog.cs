using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Monivault.AppModels
{
    public class OneCardTopupLog : Entity<long>, IHasCreationTime
    {
        public Guid OneCardTopupLogKey { get; set; }

        [Required]
        public int AccountHolderId { get; set; }

        public virtual AccountHolder AccountHolder { get; set; }

        public long? TransactionLogId { get; set; }

        public virtual TransactionLog TransactionLog { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        [StringLength(15)]
        public string AgentTransactionId { get; set; }

        [Required]
        [Column(TypeName = "varchar(11)")]
        public string Destination { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string MobileNumber { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Type { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Column(TypeName = "varchar(15)")]
        [StringLength(15)]
        public string ProductCode { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string ResultCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ResultDescription { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string ResponseValue { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string RequestCts { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string ResponseCts { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;// new DateTimeOffset(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time").GetUtcOffset(DateTime.Now)).DateTime;
    }
}
