namespace ShopFComputerBackEnd.Identity.Api.Policies
{
    public static class PermissionPolicies
    {
        public const string FullAccess = "Permission.FullAccess";
        public const string View = "Permission.View";
        public const string Create = "Permission.Create";
        public const string Update = "Permission.Update";
        public const string Delete = "Permission.Delete";
        public const string AssignUsersToRole = "Role.AssignUsersToRole";
    }
}
