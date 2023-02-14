using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users
{
    public class AddToRoleCommand : IRequest<RoleDto>
    {
        public AddToRoleCommand(Guid id, ICollection<string> roleName)
        {
            Id = id;
            RoleName = roleName;
        }
        public Guid Id { get; set; }
        public ICollection<string> RoleName { get; set; }
    }
}
