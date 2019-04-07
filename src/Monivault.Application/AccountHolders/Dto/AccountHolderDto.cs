using System;
using System.ComponentModel.DataAnnotations.Schema;
using Monivault.AppModels;
using Monivault.Banks.Dto;

namespace Monivault.AccountHolders.Dto
{
    public class AccountHolderDto
    {
        public Guid AccountHolderKey { get; set; }

        public string AccountIdentity { get; set; }
        
        public decimal AvailableBalance { get; set; }

        public decimal LedgerBalance { get; set; }

        public BankDto Bank { get; set; }
        
        public string BankAccountNumber { get; set; }

        public string BankAccountName { get; set; }
    }
}