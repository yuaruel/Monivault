using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Monivault.Authorization.Roles;
using Monivault.Authorization.Users;
using Monivault.MultiTenancy;

namespace Monivault.SignUp
{
    public class SignUpAppService : MonivaultAppServiceBase, ISignUpAppService
    {
    }
}