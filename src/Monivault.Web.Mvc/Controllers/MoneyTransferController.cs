using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monivault.AccountHolders;
using Monivault.Authorization;
using Monivault.Authorization.Users;
using Monivault.Controllers;
using Monivault.Exceptions;
using Monivault.Models;
using Monivault.ModelServices;
using Monivault.MoneyTransfers;
using Monivault.MoneyTransfers.Dto;
using Monivault.OtpSessions;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.DoBankAccountTransfer)]
    public class MoneyTransferController : MonivaultControllerBase
    {
        private readonly IAccountHolderAppService _accountHolderAppService;
        private readonly UserManager _userManager;
        private readonly IMoneyTransferAppService _moneyTransferAppService;
        private readonly NotificationScheduler _notificationScheduler;
        private readonly IOtpSessionAppService _otpSessionAppService;

        public MoneyTransferController(
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
        
        // GET
        public async Task<IActionResult> BankAccount()
        {
            ViewBag.CurrentBalance = (await _accountHolderAppService.GetAccountHolderDetail()).AvailableBalance;
            
            return View();
        }

        public async Task<PartialViewResult> GetBankTransferOtpForm(BankTransferRequestViewModel viewModel)
        {
            try
            {
                var accountHolder = await _accountHolderAppService.GetAccountHolderDetail();
                if (accountHolder.AvailableBalance < viewModel.Amount)
                {
                    ViewBag.ErrorMessage = "Insufficient funds. Please top up!";
                    return PartialView("_MoneyTransferError");
                }

                if (string.IsNullOrEmpty(accountHolder.BankAccountNumber))
                {
                    ViewBag.ErrorMessage = "You do not have a bank account in your profile. Kindly specify one.";
                    return PartialView("_MoneyTransferError");
                }
                
                var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);

                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    ViewBag.ErrorMessage = "Invalid phone number. Please update your phone number";
                    return PartialView("_MoneyTransferError");
                }

                var otp = await _moneyTransferAppService.GenerateBankAccountTransferOtp(viewModel.Amount, viewModel.Comment, user.PhoneNumber);
                await _notificationScheduler.ScheduleOtp(user.PhoneNumber, user.RealEmailAddress, otp);
                
            }
            catch (Exception exc)
            {
                Logger.Error(exc.StackTrace);
                ViewBag.ErrorMessage = "An error occurred. Please try again later";
                return PartialView("_MoneyTransferError");
            }

            return PartialView("_OtpForm");
        }

        public async Task<PartialViewResult> ValidateOtp(OtpViewModel viewModel)
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
                    PlatformSpecificDetail = "Firefox",
                    RequestOrigintingPlatform = "Web"
                };
                if(enoughBalance) await _moneyTransferAppService.TransferMoneyToAccountHolderBankAccount(transferMoneyInput);

            }
            catch (InsufficientBalanceException ibExc)
            {
                ViewBag.ErrorMessage = "Insufficient funds. Please top up!";
                return PartialView("_MoneyTransferError");
            }
            catch (InvalidOtpException ioExc)
            {
                ViewBag.ErrorMessage = "Invalid OTP";
                return PartialView("_MoneyTransferError");
            }
            catch (Exception exc)
            {
                Logger.Error(exc.StackTrace);
                ViewBag.ErrorMessage = "An error occurred. Please try again later";
                return PartialView("_MoneyTransferError");
            }

            return PartialView("_TransferSuccess");
        }
    }
}