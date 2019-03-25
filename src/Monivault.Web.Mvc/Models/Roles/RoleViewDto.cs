using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Monivault.Authorization.Roles;

namespace Monivault.Web.Models.Roles
{
    public class RoleViewDto
    {
        public Guid RoleKey { get; set; }
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public DateTime CreatedTime { get; set; }

        public IReadOnlyList<PermissionViewDto> Permissions { get; set; }
    }
}