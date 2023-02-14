using Iot.Core.Domain.Implements;
using System;
using System.Collections.Generic;
using ShopFComputerBackEnd.Order.Domain.Entities;

namespace ShopFComputerBackEnd.Order.Domain.Dtos
{
    public class OrderDto : FullAuditedEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid? ProfileId { get; set; }
        public long AmountPay { get; set; }
        public long PayingCustomer { get; set; }
        public string Description { get; set; }
        public long Payments { get; set; }
        public int StatusOrder { get; set; }
        public bool IsEnabled { get; set; }
        public ICollection<OrderDetailEntity> OrderDetailCollection { get; set; }
    }
}
