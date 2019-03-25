using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Roles;
using Monivault.Authorization.Users;

namespace Monivault.Authorization.Roles
{
    public class Role : AbpRole<User>
    {
        public const int MaxDescriptionLength = 5000;

        public Role()
        {
        }

        public Role(int? tenantId, string displayName)
            : base(tenantId, displayName)
        {
        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {
        }

        [StringLength(MaxDescriptionLength)]
        public string Description {get; set;}
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoleKey { get; set; } 
    }
}
