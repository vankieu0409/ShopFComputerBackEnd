using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos
{
    public class AssignUserToRolesDto
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> RoleId { get; set; }
    }
}
