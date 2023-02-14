using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Permissions
{
    public class GetPermissionCollectionByTypeIdQuery : IRequest<IQueryable<PermissionDto>>
    {
        public GetPermissionCollectionByTypeIdQuery(Guid typeId)
        {
            TypeId = typeId;
        }
        public Guid TypeId { get; private set; }
    }
}
