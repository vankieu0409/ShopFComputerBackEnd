using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Permissions
{
    public class GetPermissionByTypeAndFunctionQuery : IRequest<PermissionDto>
    {
        public GetPermissionByTypeAndFunctionQuery(Guid typeId, Guid functionId)
        {
            TypeId = typeId;
            FunctionId = functionId;
        }
        public Guid TypeId { get; set; }
        public Guid FunctionId { get; set; }
    }
}
