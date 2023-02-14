using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles
{
    public class GetRoleDetailByIdQuery:IRequest<RoleDto>
    {
        public GetRoleDetailByIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
