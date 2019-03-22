using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Monivault.AppModels
{
    public class TransactionLog : Entity<long>, IHasCreationTime
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionKey { get; set; }

        [Required]
        public int AccountHolderId { get; set; }
        public virtual AccountHolder AccountHolder { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(10)")]
        [StringLength(10)]
        public string TransactionType { get; set; }
        
        [Column(TypeName = "varchar(150)")]
        [StringLength(150)]
        public string Description { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(10)")]
        [StringLength(10)]
        public string RequestOriginatingPlatform { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public string PlatformSpecificDetail { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(50)")]
        [StringLength(50)]
        public string TransactionService { get; set; }
        
        [Required]
        public DateTime CreationTime { get; set; } = new DateTime();

        public class TransactionTypes
        {
            public const string Debit = "Debit";
            public const string Credit = "Credit";
        }
    }
}