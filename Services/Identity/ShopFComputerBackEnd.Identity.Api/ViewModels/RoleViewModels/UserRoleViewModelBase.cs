using System;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class UserRoleViewModelBase
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
    }
}
