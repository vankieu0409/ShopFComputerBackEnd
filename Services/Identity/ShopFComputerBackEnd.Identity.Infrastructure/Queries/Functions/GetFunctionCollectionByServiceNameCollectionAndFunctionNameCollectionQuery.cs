using ShopFComputerBackEnd.Identity.Domain.Dtos.Function;
using MediatR;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions
{
    public class GetFunctionCollectionByServiceNameCollectionAndFunctionNameCollectionQuery : IRequest<FunctionGrpcDto>
    {
        public GetFunctionCollectionByServiceNameCollectionAndFunctionNameCollectionQuery(ICollection<string> functionName, ICollection<string> serviceName)
        {
            FunctionName = functionName;
            ServiceName = serviceName;
        }

        public ICollection<string> FunctionName { get; set; }
        public ICollection<string> ServiceName { get; set; }
    }
}
