using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Monivault.MultiTenancy.Dto;

namespace Monivault.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

