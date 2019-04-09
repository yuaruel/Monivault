using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Configuration;
using Monivault.AccountHolders;
using Monivault.Authorization.Users;
using Monivault.Controllers;
using Monivault.ModelServices;
using Monivault.MoneyTransfers;
using Monivault.OtpSessions;
using Monivault.Web.Models.MoneyTransfer;

namespace Monivault.Web.Controllers
{
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
            Debug.WriteLine("Getting OTP Form...");
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
                //_notificationScheduler.ScheduleOtp(user.PhoneNumber, user.RealEmailAddress, otp);
                
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
                var otpSession = _otpSessionAppService.GetOtpSession(viewModel.Otp);

                var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);

                if (otpSession.PhoneNumberSentTo != user.PhoneNumber)
                {
                    ViewBag.ErrorMessage = "Invalid OTP";
                    return PartialView("_MoneyTransferError");
                }
                
                var accountHolder = await _accountHolderAppService.GetAccountHolderDetail();
                if (accountHolder.AvailableBalance < decimal.Parse(otpSession.ActionProperty["Amount"]))
                {
                    ViewBag.ErrorMessage = "Insufficient funds. Please top up!";
                    return PartialView("_MoneyTransferError");
                }

                var amount = otpSession.ActionProperty["Amount"].Split(".")[0].Trim();
                await _moneyTransferAppService.TransferMoneyToBankAccount(amount, accountHolder.Bank.OneCardBankCode, accountHolder.BankAccountNumber, 
                    user.PhoneNumber);

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