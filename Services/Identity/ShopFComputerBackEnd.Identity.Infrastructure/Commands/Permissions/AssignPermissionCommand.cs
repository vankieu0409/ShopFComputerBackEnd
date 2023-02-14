using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using ShopFComputerBackEnd.Identity.Domain.Enums;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Permissions
{
    public class AssignPermissionCommand : IRequest<PermissionDto>
    {
        public AssignPermissionCommand(Guid typeId, Guid functionId)
        {
            TypeId = typeId;
            FunctionId = functionId;
        }
        public PermissionType Type { get; set; }
        public Guid TypeId { get; set; }
        public Guid FunctionId { get; set; }
    }
}
