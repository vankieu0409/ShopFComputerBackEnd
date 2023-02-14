using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions
{
    public  class UpdateFunctionCollectionCommand : IRequest<List<FunctionDto>>
    {
        public UpdateFunctionCollectionCommand(List<UpdateFunctionCommand> commandCollection)
        {
            CommandCollection = commandCollection;
        }
        public List<UpdateFunctionCommand> CommandCollection { get; set; }   
    }
}
