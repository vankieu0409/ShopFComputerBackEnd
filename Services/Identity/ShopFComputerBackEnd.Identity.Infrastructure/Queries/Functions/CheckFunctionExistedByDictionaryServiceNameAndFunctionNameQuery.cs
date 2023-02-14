using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.Dtos.Function;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions
{
    public class CheckFunctionExistedByDictionaryServiceNameAndFunctionNameQuery : IRequest<IQueryable<CheckFunctionExistedDto>>
    {
        public CheckFunctionExistedByDictionaryServiceNameAndFunctionNameQuery(Dictionary<string, IEnumerable<string>> dictionaryFunction)
        {
            DictionaryFunction = dictionaryFunction;
        }

        public Dictionary<string, IEnumerable<string>> DictionaryFunction { get; set; }
    }
}
