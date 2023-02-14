using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos.Function
{
    public class FunctionGrpcDto
    {
        public List<FunctionDto> FunctionToAdd { get; set; }
        public List<FunctionDto> FunctionToUpdate { get; set; }
    }
}
