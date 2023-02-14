using Iot.Core.Domain.Implements;
using System;
using System.Collections.Generic;
using ShopFComputerBackEnd.Order.Domain.Entities;

namespace ShopFComputerBackEnd.Order.Domain.ReadModels
{
    public class OrderReadModel : FullEntity<Guid>
    {
        public Guid ProfileId { get; set; }
        public Int64 AmountPay { get; set; }
        public Int64 PayingCustomer { get; set; }
        public string? Description { get; set; }
        public Int64 Payments { get; set; }
        public int StatusOrder { get; set; }
        public bool IsEnabled { get; set; }
        public virtual ICollection<OrderDetailEntity> OrderDetails { get; set; }
    }
}
