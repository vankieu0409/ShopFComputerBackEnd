using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos.Function
{
    public class CheckFunctionExistedDto
    {
        public string Key { get; set; }
        public IEnumerable<string> Value { get; set; }
    }
}
