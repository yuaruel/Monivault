using System.Threading.Tasks;
using Monivault.Configuration.Dto;

namespace Monivault.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
