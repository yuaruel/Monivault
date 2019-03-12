using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Monivault.Models
{
    public class VerificationCode : Entity, IHasCreationTime
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        public string PhoneNumber { get; set; }
        
        public DateTime CreationTime { get; set; }
    }
}