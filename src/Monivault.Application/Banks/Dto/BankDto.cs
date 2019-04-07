using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Monivault.AppModels;

namespace Monivault.Banks.Dto
{
    [AutoMap(typeof(Bank))]
    public class BankDto
    {
        public Guid BankKey { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string OneCardBankCode { get; set; }
    }
}