using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions
{
    public class DeleteFunctionCommand:IRequest<FunctionDto>
    {
        public DeleteFunctionCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
