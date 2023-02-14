namespace ShopFComputerBackEnd.Identity.Api.Policies
{
    public static class UserPolicies
    {
        public const string FullAccess = "User.FullAccess";
        public const string View = "User.View";
        public const string Create = "User.Create";
        public const string Update = "User.Update";
        public const string Delete = "User.Delete";
        public const string AssignRolesToUser = "User.AssignRolesToUser";
    }
}
