using MediatR;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles
{
    public class GetRoleNameCollectionByIdCollectionQuery : IRequest<List<string>>
    {
        public GetRoleNameCollectionByIdCollectionQuery(IEnumerable<Guid> roleIdCollection)
        {
            RoleIdCollection = roleIdCollection;
        }

        public IEnumerable<Guid> RoleIdCollection { get; private set; }
    }
}
