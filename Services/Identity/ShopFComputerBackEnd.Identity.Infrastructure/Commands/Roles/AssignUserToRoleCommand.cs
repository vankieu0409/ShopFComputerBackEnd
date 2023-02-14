using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles
{
    public class AssignUserToRoleCommand : IRequest<AssignUserToRoleDto>
    {
        public AssignUserToRoleCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
