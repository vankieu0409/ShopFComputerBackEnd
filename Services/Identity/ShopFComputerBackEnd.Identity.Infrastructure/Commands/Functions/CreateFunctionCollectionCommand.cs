using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions
{
    public class CreateFunctionCollectionCommand : IRequest<ICollection<FunctionDto>>
    {
        public CreateFunctionCollectionCommand(ICollection<CreateFunctionCommand> commandCollection)
        {
            CommandCollection = commandCollection;
        }
        public ICollection<CreateFunctionCommand> CommandCollection { get; set; }
    }
}
