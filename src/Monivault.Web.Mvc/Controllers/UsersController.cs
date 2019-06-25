using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.FileProviders;
using Monivault.Authorization;
using Monivault.Controllers;
using Monivault.Net.MimeTypes;
using Monivault.Roles;
using Monivault.Roles.Dto;
using Monivault.Users;
using Monivault.Web.Models.Users;
using Monivault.Users.Dto;
using Monivault.Web.Models.Roles;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.ViewUsers)]
    public class UsersController : MonivaultControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;

        public UsersController(
                IUserAppService userAppService,
                IRoleAppService roleAppService
            )
        {
            _userAppService = userAppService;
            _roleAppService = roleAppService;

        }

        public async Task<ActionResult> Index()
        {
            //TODO Implement paging in the GetAll() of userAppService.
            var users = (await _userAppService.GetAll(new PagedUserResultRequestDto {MaxResultCount = int.MaxValue})).Items; // Paging not implemented yet
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new UserListViewModel
            {
                Users = users,
                Roles = roles
            };
            return View(model);
        }

        public async Task<JsonResult> UserList()
        {
            //var users = (await _userAppService.GetAll(new PagedUserResultRequestDto {MaxResultCount = int.MaxValue})).Items;
            var users = await _userAppService.GetUserList();
            
            return Json(users.Items);
        }

        public async Task<PartialViewResult> CreateUserModal()
        {
            var roles = (await _roleAppService.GetRolesAsync(new GetRolesInput())).Items;;
            var viewModel = new RoleListViewModel
            {
                Roles = roles
            };
            
            return PartialView("_CreateUserModal", viewModel);
        }

        public async Task<JsonResult> CreateUser(CreateUserDto input)
        {
            input.Password = Authorization.Users.User.CreateRandomPassword();
            await _userAppService.Create(input);

            return Json(new { });
        }

        public async Task<JsonResult> UpdateUser(EditUserDto userDto)
        {
            await _userAppService.UpdateUser(userDto);

            return Json(new { });
        }

        public async Task<PartialViewResult> EditUserModal(string userKey)
        {  
            var user = _userAppService.GetUserByKey(userKey);
            var roles = (await _userAppService.GetRoles()).Items;
            
            var model = new EditUserModalViewModel
            {
                User = user,
                Roles = roles
            };
            return PartialView("_EditUserModal", model);
        }
    }
}
