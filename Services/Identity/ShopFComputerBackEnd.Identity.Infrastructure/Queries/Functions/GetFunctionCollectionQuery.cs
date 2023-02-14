using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System.Linq;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions
{
    public class GetFunctionCollectionQuery : IRequest<IQueryable<FunctionDto>>
    {
        public GetFunctionCollectionQuery()
        {
        }
    }
}
