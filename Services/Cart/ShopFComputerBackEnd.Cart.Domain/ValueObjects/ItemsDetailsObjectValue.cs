using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Cart.Domain.ValueObjects
{
    public class ItemsDetailsObjectValue : CartItemValueObject
    {
        public string SkuId { get; set; }
        public string Name { get; set; }
        public string OptionValues { get; set; }
        public long ImportPrice { get; set; }
        public long Price { get; set; }
        public string Url { get; set; }
        public string Brand { get; set; }
        public string Caregory { get; set; }
    }
}
