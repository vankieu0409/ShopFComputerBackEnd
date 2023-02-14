using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions
{
    public class CreateFunctionCommand : IRequest<FunctionDto>
    {
        public CreateFunctionCommand(Guid id)
        {
            Id = id;
   
        }
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string FunctionName { get; set; }
    }
}
