using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Permissions
{
    public class GetPermissionCollectionByTypeIdCollectionQuery : IRequest<IQueryable<PermissionDto>>
{
        public GetPermissionCollectionByTypeIdCollectionQuery(ICollection<Guid> typeId)
    {
            TypeId = typeId;
        }

        public ICollection<Guid> TypeId { get; set; }
    }
}