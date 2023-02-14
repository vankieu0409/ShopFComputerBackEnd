using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles
{
    public class GetRoleDetailByNameQuery : IRequest<RoleDto>
    {
        public GetRoleDetailByNameQuery(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
