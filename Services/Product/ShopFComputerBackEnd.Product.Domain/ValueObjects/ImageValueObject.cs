using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Domain.ValueObjects
{
    public class ImageValueObject
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Status => "done";
    }
}
