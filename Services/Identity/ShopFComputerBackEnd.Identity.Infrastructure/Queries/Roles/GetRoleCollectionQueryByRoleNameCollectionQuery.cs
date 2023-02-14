using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles
{
    public class GetRoleCollectionQueryByRoleNameCollectionQuery:IRequest<IQueryable<RoleDto>>
    {
        public GetRoleCollectionQueryByRoleNameCollectionQuery(List<string> roleNameCollection)
        {
            RoleNameCollection = roleNameCollection;
        }
        public List<string> RoleNameCollection { get; set; }
    }
}
