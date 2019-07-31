using Abp.Runtime.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monivault.Authorization;
using Monivault.AccountHolders;
using Monivault.Authorization.Users;
using Monivault.Controllers;
using Monivault.Models;
using Monivault.ModelServices;
using Monivault.MoneyTransfers;
using Monivault.OtpSessions;
using Monivault.MoneyTransfers.Dto;
using Monivault.Exceptions;
using Monivault.Configuration;
using Abp.UI;

namespace Monivault.Web.Host.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MoneyTransferServiceController : MonivaultControllerBase
    {
        private readonly IAccountHolderAppService _accountHolderAppService;
        private readonly UserManager _userManager;
        private readonly IMoneyTransferAppService _moneyTransferAppService;
        private readonly NotificationScheduler _notificationScheduler;
        private readonly IOtpSessionAppService _otpSessionAppService;

        public MoneyTransferServiceController(
                IAccountHolderAppService accountHolderAppService,
                UserManager userManager,
                IMoneyTransferAppService moneyTransferAppService,
                NotificationScheduler notificationScheduler,
                IOtpSessionAppService otpSessionAppService
            )
        {
            _accountHolderAppService = accountHolderAppService;
            _userManager = userManager;
            _moneyTransferAppService = moneyTransferAppService;
            _notificationScheduler = notificationScheduler;
            _otpSessionAppService = otpSessionAppService;
        }

        [HttpPost]
        public async Task<JsonResult> RequestTransfer([FromBody] BankTransferRequestViewModel viewModel)
        {
            var transferDisabled = bool.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.StopWithdrawal));

            if (transferDisabled) throw new UserFriendlyException("TransferDisbaled");

            try
            {
                var accountHolder = await _accountHolderAppService.GetAccountHolderDetail();
                if (accountHolder.AvailableBalance < viewModel.Amount)
                {
                    return Json("Insufficient funds. Please top up!");
                }

                if (string.IsNullOrEmpty(accountHolder.BankAccountNumber))
                {
                    //ViewBag.ErrorMessage = "You do not have a bank account in your profile. Kindly specify one.";
                    return Json("You do not have a bank account in your profile. Kindly specify one.");
                }
                
                var user = await _userManager.GetUserByIdAsync(User.Identity.GetUserId().Value);

                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    //ViewBag.ErrorMessage = "Invalid phone number. Please update your phone number";
                    return Json("Invalid phone number. Please update your phone number");
                }

                var otp = await _moneyTransferAppService.GenerateBankAccountTransferOtp(viewModel.Amount, viewModel.Comment, user.PhoneNumber);
                await _notificationScheduler.ScheduleOtp(user.PhoneNumber, user.RealEmailAddress, otp);

            }
            catch (Exception exc)
            {
                Logger.Error(exc.StackTrace);
                //ViewBag.ErrorMessage = "An error occurred. Please try again later";
                return Json("An error occurred. Please try again later");
            }

            return Json(new { });
        }

        [HttpPost]
        public async Task<JsonResult> ValidateOtp([FromBody] OtpViewModel viewModel)
        {
            try
            {
                //Validate OTP
                var otpSession = await _otpSessionAppService.ValidateOtp(viewModel.Otp);

                var amount = decimal.Parse(otpSession.ActionProperty["Amount"].Split(".")[0].Trim());

                //Check if AccountHolder has sufficient balance.
                var enoughBalance = await _accountHolderAppService.IsAvailableBalanceEnough(amount);

                var transferMoneyInput = new TransferMoneyToAccountInput
                {
                    Amount = amount,
                    PlatformSpecificDetail = "Android",
                    RequestOrigintingPlatform = "Mobile"
                };
                if (enoughBalance) await _moneyTransferAppService.TransferMoneyToAccountHolderBankAccount(transferMoneyInput);

            }
            catch (InsufficientBalanceException ibExc)
            {
                ViewBag.ErrorMessage = "Insufficient funds. Please top up!";
                return Json("Insufficient funds. Please top up");
            }
            catch (InvalidOtpException ioExc)
            {
                ViewBag.ErrorMessage = "Invalid OTP";
                return Json("Invalid OTP");
            }
            catch (Exception exc)
            {
                Logger.Error(exc.StackTrace);
                ViewBag.ErrorMessage = "An error occurred. Please try again later";
                return Json("An error occurred. Please try again later");
            }

            return Json("Successful Transfer");
        }
    }
}