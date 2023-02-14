using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class AssignUserToRolesViewModel
    {
        public IEnumerable<Guid> RoleIds { get; set; }
    }
}
