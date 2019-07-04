namespace Monivault.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        public static class Tenants
        {
            public const string Admin = "SuperAdmin";
            public const string AccountHolder = "AccountHolder";
            public const string AccountOfficer = "AccountOfficer";
        }
    }
}
