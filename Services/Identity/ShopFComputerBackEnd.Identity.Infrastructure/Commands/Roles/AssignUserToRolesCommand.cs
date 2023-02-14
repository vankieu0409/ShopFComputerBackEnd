using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles
{
    public class AssignUserToRolesCommand : IRequest<AssignUserToRolesDto>
    {
        public AssignUserToRolesCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public IEnumerable<Guid> RoleIds { get; set; }
    }
}
