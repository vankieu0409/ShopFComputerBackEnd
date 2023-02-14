using ShopFComputerBackEnd.Identity.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions
{
    public class GetFunctionDetailByServiceAndFuntionQuery:IRequest<FunctionDto>
    {
        public GetFunctionDetailByServiceAndFuntionQuery(string serviceName, string functionName)
        {
            ServiceName = serviceName;
            FunctionName = functionName;
        }
        public string ServiceName { get; private set; }
        public string FunctionName { get; private set; }
    }
}
