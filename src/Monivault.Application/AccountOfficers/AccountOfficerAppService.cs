using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Monivault.AccountOfficers.Dto;
using Monivault.Authorization.Roles;
using Monivault.Authorization.Users;

namespace Monivault.AccountOfficers
{
    public class AccountOfficerAppService : MonivaultAppServiceBase, IAccountOfficerAppService
    {
        private readonly UserManager _userManager;

        public AccountOfficerAppService(
                UserManager userManager
            )
        {
            _userManager = userManager;
        }

        public async Task<List<AccountOfficerDto>> GetAccountOfficers()
        {
            var accountOfficerUsers = await _userManager.GetUsersInRoleAsync(StaticRoleNames.Tenants.AccountOfficer);

            var accountOfficers = new List<AccountOfficerDto>();

            foreach(var user in accountOfficerUsers)
            {
                var accountOfficerDto = new AccountOfficerDto
                {
                    AccountOfficerId = user.Id,
                    FirstName = user.Name,
                    LastName = user.Surname
                };

                accountOfficers.Add(accountOfficerDto);
            }

            return accountOfficers;
        }
    }
}
