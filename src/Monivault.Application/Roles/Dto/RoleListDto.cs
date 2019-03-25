using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

namespace Monivault.Roles.Dto
{
    public class RoleListDto : IHasCreationTime
    {
        public Guid RoleKey { get; set; }
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
