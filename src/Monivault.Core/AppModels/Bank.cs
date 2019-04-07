using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace Monivault.AppModels
{
    public class Bank : Entity
    {
        [Required] public Guid BankKey { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        public string OneCardBankCode { get; set; }
    }
}