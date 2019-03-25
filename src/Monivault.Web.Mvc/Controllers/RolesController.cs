using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AutoMapper;
using Monivault.Authorization;
using Monivault.Authorization.Roles;
using Monivault.Controllers;
using Monivault.Roles;
using Monivault.Roles.Dto;
using Monivault.Web.Models.Roles;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_RoleManagement)]
    public class RolesController : MonivaultControllerBase
    {
        private readonly IRoleAppService _roleAppService;
        private readonly RoleManager _roleManager;

        public RolesController(
                IRoleAppService roleAppService,
                RoleManager roleManager
            )
        {
            _roleAppService = roleAppService;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = (await _roleAppService.GetRolesAsync(new GetRolesInput())).Items;
            var permissions = (await _roleAppService.GetAllPermissions()).Items;
            var model = new RoleListViewModel
            {
                Roles = roles,
                Permissions = permissions
            };

            return View(model);
        }

        public async Task<ActionResult> EditRoleModal(int roleId)
        {
            var output = await _roleAppService.GetRoleForEdit(new EntityDto(roleId));
            var model = new EditRoleModalViewModel(output);

            return View("_EditRoleModal", model);
        }

        public PartialViewResult AddRoleModal()
        {
            return PartialView("_AddRoleModal");
        }

        [AbpMvcAuthorize(PermissionNames.Pages_RoleManagement)]
        public async Task<JsonResult> AllRoles()
        {
            var roleListDto = new List<RoleViewDto>();
            
            var roles = _roleManager.Roles.ToList();
            foreach (var role in roles)
            {
                Logger.Info("Role name: " + role.Name);
                var permissions = await _roleManager.GetGrantedPermissionsAsync(role);
                var permissionList = permissions.Select(permission => new PermissionViewDto {Name = permission.Name}).ToList();

                roleListDto.Add(new RoleViewDto
                {
                    RoleKey = role.RoleKey,
                    Name = role.Name,
                    DisplayName = role.DisplayName,
                    CreatedTime = role.CreationTime,
                    Permissions = new ListResultDto<PermissionViewDto>(permissionList).Items
                }); 
            }
            
            return Json(roleListDto);
        }
    }
}
