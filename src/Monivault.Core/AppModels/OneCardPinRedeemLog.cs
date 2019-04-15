using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Monivault.AppModels
{
    public class OneCardPinRedeemLog : Entity<long>, IHasCreationTime
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OneCardPinRedeemKey { get; set; } = Guid.NewGuid();

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
        [Column(TypeName = "varchar(16)")]
        [StringLength(16)]
        public string PinNo { get; set; }

        [Column(TypeName = "varchar(20)")]
        [StringLength(20)]
        public string SerialNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        [StringLength(50)]
        public string ServiceId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Column(TypeName = "varchar(150)")]
        [StringLength(150)]
        public string Comments { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        [StringLength(15)]
        public string VendorCode { get; set; }

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

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}