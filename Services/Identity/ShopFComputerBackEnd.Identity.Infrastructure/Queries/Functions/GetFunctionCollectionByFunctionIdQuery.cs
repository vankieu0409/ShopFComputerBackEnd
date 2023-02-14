using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions
{
    public class GetFunctionCollectionByFunctionIdQuery : IRequest<IQueryable<FunctionDto>>
    {
        public GetFunctionCollectionByFunctionIdQuery(ICollection<Guid> functionIdCollection)
        {
            FunctionIdCollection = functionIdCollection;
        }

        public ICollection<Guid> FunctionIdCollection { get; set; }
    }
}
