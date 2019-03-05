using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Monivault.Roles.Dto;
using Monivault.Users.Dto;

namespace Monivault.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
