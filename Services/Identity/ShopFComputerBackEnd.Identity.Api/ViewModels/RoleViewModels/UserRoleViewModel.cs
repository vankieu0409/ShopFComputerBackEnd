using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class UserRoleViewModel
    {
        public ICollection<UserRoleViewModelBase> UserRoles { get; set; }
    }
}
