using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Monivault.Configuration;
using Monivault.Controllers;
using Monivault.Web.Models.Settings;

namespace Monivault.Web.Controllers
{
    public class SettingsController : MonivaultControllerBase
    {
        // GET
        public async Task<IActionResult> Index()
        {
            var settings = await SettingManager.GetAllSettingValuesAsync();
            var viewModel = new SettingViewModel();
 
            foreach (var setting in settings)
            {
                switch (setting.Name)
                {
                    case AppSettingNames.StopDeposit:
                        viewModel.StopDeposit = bool.Parse(setting.Value);
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
            SettingManager.ChangeSettingForApplication(AppSettingNames.StopDeposit, viewModel.StopDeposit.ToString());
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
            Logger.Info("Interest Status: " + viewModel.InterestStatus);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestStatus,viewModel.InterestStatus.ToString());
            if (!viewModel.InterestStatus) return Json(new { });
            
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestType,
                viewModel.InterestType);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestRate,
                viewModel.InterestRate.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InterestDuration,
                viewModel.InterestDuration.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.PenaltyPercentageDeduction,
                viewModel.PenaltyDeduction.ToString());

            return Json(new { });
        }
    }
}