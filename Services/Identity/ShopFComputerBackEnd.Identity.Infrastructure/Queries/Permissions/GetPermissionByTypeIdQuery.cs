using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Permissions
{
    public class GetPermissionByTypeIdQuery : IRequest<PermissionDto>
    {
        public GetPermissionByTypeIdQuery(Guid typeId)
        {
            TypeId = typeId;
        }

        public Guid TypeId { get; set; }
    }
}
