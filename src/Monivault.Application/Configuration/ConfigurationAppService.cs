using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Monivault.Configuration.Dto;

namespace Monivault.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : MonivaultAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
