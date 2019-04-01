using System;
using Abp.Domain.Entities.Auditing;

namespace Monivault.Users.Dto
{
    public class UserListDto : IHasCreationTime
    {
        public Guid UserKey { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string[] RoleNames { get; set; }
        
        public DateTime CreationTime { get; set; }
    }
}