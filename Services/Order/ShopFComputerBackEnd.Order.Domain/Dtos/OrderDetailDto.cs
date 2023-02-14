using Iot.Core.Domain.Implements;
using System;

namespace ShopFComputerBackEnd.Order.Domain.Dtos
{
    public class OrderDetailDto : FullEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid VariantId { get; set; }
        public int Quantity { get; set; }
        public long UnitPrice { get; set; }
        public bool IsEnabled { get; set; }
    }
}
