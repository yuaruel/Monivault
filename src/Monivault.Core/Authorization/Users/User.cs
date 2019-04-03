using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using Monivault.Utils;

namespace Monivault.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "pass1word@";

        private string _emailAddress;
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserKey { get; set; }

        public override string EmailAddress
        {
            get => _emailAddress;
            set => _emailAddress = string.IsNullOrEmpty(value) ? RandomStringGeneratorUtil.GenerateFakeEmail() : value;
        }

        /// <summary>
        /// This is the email property that will be used in this application.
        /// The mail EmailAddress, will contain a fake email if user did not specify any email, this is because
        /// the aspnetboilerplate framework requires a unique email for all users. But the Monivault requirement allows for no email.
        /// </summary>
        [NotMapped]
        public string RealEmailAddress
        {
            get
            {
                return EmailAddress.Contains("@fakeemailforapp.com") ? "" : EmailAddress;
            } 
        }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
