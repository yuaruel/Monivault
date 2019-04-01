using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AutoMapper;
using Abp.Localization;
using Monivault.Authorization;
using Monivault.Authorization.Roles;
using Monivault.Controllers;
using Monivault.Roles;
using Monivault.Roles.Dto;
using Monivault.Web.Models.Roles;

namespace Monivault.Web.Controllers
{
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

        [AbpMvcAuthorize(PermissionNames.ViewRoles)]
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

        public async Task<PartialViewResult> EditRoleModal(string roleKey)
        {
            var output = await _roleAppService.GetRoleForEdit(roleKey);
            var model = new EditRoleModalViewModel(output);

            return PartialView("_EditRoleModal", model);
        }

        public async Task<JsonResult> UpdateRole(RoleDto roleDto)
        {
            var upateRole = await _roleAppService.Update(roleDto);

            return Json(new { });
        }

        public async Task<PartialViewResult> CreateRoleModal()
        {
            var roles = (await _roleAppService.GetRolesAsync(new GetRolesInput())).Items;
            var permissions = (await _roleAppService.GetAllPermissions()).Items;
            var viewModel = new PermissionListViewModel()
            {
                Permissions = permissions
            };
            
            return PartialView("_CreateRoleModal", viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> CreateRole(CreateRoleDto roleDto)
        {
            var createdRole = await _roleAppService.Create(roleDto);
            
            return Json(new { });
        }

        [AbpMvcAuthorize(PermissionNames.ViewRoles)]
        public async Task<JsonResult> AllRoles()
        {
            var roleListDto = new List<RoleViewDto>();
            
            var roles = _roleManager.Roles.ToList();
            foreach (var role in roles)
            {
                var permissions = await _roleManager.GetGrantedPermissionsAsync(role);
                var permissionList = permissions.Select(permission => new PermissionViewDto {DisplayName = (permission.DisplayName.Localize(new LocalizationContext(LocalizationManager)))}).ToList();

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
