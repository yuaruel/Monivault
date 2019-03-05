using System.Collections.Generic;
using Monivault.Roles.Dto;

namespace Monivault.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}