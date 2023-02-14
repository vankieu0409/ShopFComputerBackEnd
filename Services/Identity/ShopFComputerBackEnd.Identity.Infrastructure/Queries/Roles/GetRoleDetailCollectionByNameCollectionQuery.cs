using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles
{
    public class GetRoleDetailCollectionByNameCollectionQuery : IRequest<IQueryable<RoleDto>>
    {
        public GetRoleDetailCollectionByNameCollectionQuery(ICollection<string> nameCollection)
        {
            NameCollection = nameCollection;
        }

        public ICollection<string> NameCollection { get; set; }
    }
}
