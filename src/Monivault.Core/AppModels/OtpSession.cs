using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Monivault.Authorization.Users;

namespace Monivault.AppModels
{
    public class OtpSession : Entity, IHasCreationTime
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string PhoneNumberSentTo { get; set; }

        [Required]
        public long UserId { get; set; }
        public virtual User User { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public Dictionary<string, string> ActionProperty { get; set; }
        
        public DateTime CreationTime { get; set; } =  new DateTime();
    }
}