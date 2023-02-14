using System;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos.Permission
{
    public class AssignRoleDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
