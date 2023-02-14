using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles
{
    public class CreateRoleCommand : IRequest<RoleDto>
    {
        public CreateRoleCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
