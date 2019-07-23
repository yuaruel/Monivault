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
using Abp.BackgroundJobs;
using Monivault.BackgroundJobs.Email;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.ViewUsers)]
    public class UsersController : MonivaultControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public UsersController(
                IUserAppService userAppService,
                IRoleAppService roleAppService,
                IBackgroundJobManager backgroundJobManager
            )
        {
            _userAppService = userAppService;
            _roleAppService = roleAppService;
            _backgroundJobManager = backgroundJobManager;
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

        public JsonResult UserList()
        {
            //var users = (await _userAppService.GetAll(new PagedUserResultRequestDto {MaxResultCount = int.MaxValue})).Items;
            var users = _userAppService.GetUserList();
            
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

            //Queue email job
            await _backgroundJobManager.EnqueueAsync<UserAccountCreatedEmailJob, UserAccountCreatedEmailJobArgs>(new UserAccountCreatedEmailJobArgs
            {
                UserEmail = input.EmailAddress,
                UserPassword = input.Password,
                UserName = input.UserName,
                FullName = $"{input.Name} {input.Surname}"
            });

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
