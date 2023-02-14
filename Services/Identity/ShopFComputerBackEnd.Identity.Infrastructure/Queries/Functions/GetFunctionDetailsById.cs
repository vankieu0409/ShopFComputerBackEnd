using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions
{
    public class GetFunctionDetailsById : IRequest<FunctionDto>
    {
        public GetFunctionDetailsById(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
