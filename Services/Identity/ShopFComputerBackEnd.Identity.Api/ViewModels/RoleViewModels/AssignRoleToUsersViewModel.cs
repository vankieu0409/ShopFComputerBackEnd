using ShopFComputerBackEnd.Identity.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Api.ViewModels
{
    public class AssignRoleToUsersViewModel
    {
        public IEnumerable<Guid> UserIds { get; set; }
    }
}
