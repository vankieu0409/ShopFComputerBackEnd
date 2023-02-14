using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles
{
    public class GetRoleDetailCollectionByRoleIdCollectionQuery : IRequest<IQueryable<RoleDto>>
    {
        public GetRoleDetailCollectionByRoleIdCollectionQuery(ICollection<Guid> roleId)
        {
            RoleId = roleId;
        }

        public ICollection<Guid> RoleId { get; set; }
    }
}
