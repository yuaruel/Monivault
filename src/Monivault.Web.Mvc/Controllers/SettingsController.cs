using System;
using System.Globalization;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Monivault.Configuration;
using Monivault.Controllers;
using Monivault.SavingsInterests;
using Monivault.Web.Models.Settings;

namespace Monivault.Web.Controllers
{
    public class SettingsController : MonivaultControllerBase
    {
        private readonly SavingsInterestManager _savingsInterestManager;

        public SettingsController(
                SavingsInterestManager savingsInterestManager
            )
        {
            _savingsInterestManager = savingsInterestManager;
        }
        
        // GET
        public async Task<IActionResult> Index()
        {
            var settings = await SettingManager.GetAllSettingValuesAsync();
            var viewModel = new SettingViewModel();
 
            foreach (var setting in settings)
            {
                switch (setting.Name)
                {
                    case AppSettingNames.StopTopUpSaving:
                        viewModel.StopTopUpSaving = bool.Parse(setting.Value);
                        break;
                    case AppSettingNames.StopSignUp:
                        viewModel.StopSignUp = bool.Parse(setting.Value);
                        break;
                    case AppSettingNames.StopWithdrawal:
                        viewModel.StopWithdrawal = bool.Parse(setting.Value);
                        break;
                    case AppSettingNames.WithdrawalServiceCharge:
                        viewModel.WithdrawalServiceCharge = decimal.Parse(setting.Value);
                        break;
                    case AppSettingNames.InterestStatus:
                        viewModel.InterestStatus = bool.Parse(setting.Value);
                        break;
                    case AppSettingNames.InterestType:
                        viewModel.InterestType = setting.Value;
                        break;
                    case AppSettingNames.InterestRate:
                        viewModel.InterestRate = decimal.Parse(setting.Value);
                        break;
                    case AppSettingNames.InterestDuration:
                        viewModel.InterestDuration = int.Parse(setting.Value);
                        break;
                    /*case AppSettingNames.InterestDurationStartDate:
                        viewModel.InterestDurationStartDate = DateTime.ParseExact(setting.Value, "dd/MM/yyyy", new CultureInfo("en-GB"));
                        break;
                    case AppSettingNames.InterestDurationEndDate:
                        viewModel.InterestDurationEndDate = DateTime.ParseExact(setting.Value, "dd/MM/yyyy", new CultureInfo("en-GB"));
                        break;*/
                    case AppSettingNames.PenaltyPercentageDeduction:
                        viewModel.PenaltyDeduction = decimal.Parse(setting.Value);
                        break;
                }
            }
            
            return View(viewModel);
        }

        public JsonResult UpdateGeneralSettings([FromBody]SettingViewModel viewModel)
        {
            //Change general settings
            SettingManager.ChangeSettingForApplication(AppSettingNames.StopTopUpSaving, viewModel.StopTopUpSaving.ToString());
            SettingManager.ChangeSettingForApplication(AppSettingNames.StopSignUp, viewModel.StopSignUp.ToString());
            
            return Json(new { });
        }

        public async Task<JsonResult> UpdateWithdrawalSettings([FromBody]SettingViewModel viewModel)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.StopWithdrawal,viewModel.StopWithdrawal.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.WithdrawalServiceCharge, viewModel.WithdrawalServiceCharge.ToString());
            
            return Json(new { });
        }

        public async Task<JsonResult> UpdateInterestSettings([FromBody]SettingViewModel viewModel)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestStatus,viewModel.InterestStatus.ToString());
            if (!viewModel.InterestStatus)
            {
                SavingsInterestManager.StopSavingsInterestProcessing();
                return Json(new { });
            }
            
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestType,
                viewModel.InterestType);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestRate,
                viewModel.InterestRate.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestDuration,
                viewModel.InterestDuration.ToString());
            /*await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestDurationStartDate,
                viewModel.InterestDurationStartDate.ToString("dd/MM/yyyy"));
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestDurationEndDate,
                viewModel.InterestDurationEndDate.ToString("dd/MM/yyyy"));*/
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.PenaltyPercentageDeduction,
                viewModel.PenaltyDeduction.ToString());
            
            //Bootstrap SavingsInterest for all accountHolders.
            await _savingsInterestManager.BootstrapNewSavingsInterestForAllAccountHolders();
            SavingsInterestManager.StartSavingsInterestProcessing();

            return Json(new { });
        }
    }
}