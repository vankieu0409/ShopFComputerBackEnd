using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Permissions
{
    public class AssignPermissionCollectionCommand : IRequest<ICollection<PermissionDto>>
    {
        public AssignPermissionCollectionCommand(ICollection<AssignPermissionCommand> commandCollection)
        {
            CommandCollection = commandCollection;
        }
        public ICollection<AssignPermissionCommand> CommandCollection { get; private set; }
    }
}
