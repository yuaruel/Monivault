using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Monivault.AppModels
{
    public class ResetPasswordToken : Entity<string>, IHasCreationTime
    {
        [Required]
        public override string Id { get; set; }

        public long UserId { get; set; }

        public string EmailOrPhoneNumber { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}
