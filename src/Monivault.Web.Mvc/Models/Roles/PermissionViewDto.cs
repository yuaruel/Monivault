using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.AutoMapper;

namespace Monivault.Web.Models.Roles
{
    public class PermissionViewDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}