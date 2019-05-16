using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Monivault.MoneyTransfers.Dto
{
    public class TransferMoneyToAccountInput
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string RequestOrigintingPlatform { get; set; }   //The platform from which the request originated from. Web | Mobile

        [Required]
        public string PlatformSpecificDetail { get; set; }  //The specific originating platform. Firefox|Chrome|Android|iPhone etc.
    }
}
