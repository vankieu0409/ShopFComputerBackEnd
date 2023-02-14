using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Domain.ValueObjects
{
    public class FunctionValueObject
    {
        public IDictionary<string, IEnumerable<string>> FunctionDictionary { get; set; }
    }
}
