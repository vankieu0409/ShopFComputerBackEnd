using ShopFComputerBackEnd.Identity.Domain.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos.Role
{
    public class AssignRoleToUserDto
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }
        public List<UserDto> UserDtos { get; set; }
    }
}
