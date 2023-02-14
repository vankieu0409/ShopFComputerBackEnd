using Iot.Core.Domain.Events;

using System.Collections.Generic;
using System;
using ShopFComputerBackEnd.Order.Domain.Entities;

namespace ShopFComputerBackEnd.Order.Domain.Events.OrderDetails;

public class OrderDetailsUpdateCollectionEvent : EventBase
{
    public OrderDetailsUpdateCollectionEvent(Guid orderId, ICollection<OrderDetailEntity> oderDetailsCollection, Guid? modifiedBy)
    {
        OrderId = orderId;
        OderDetailsCollection = oderDetailsCollection;
        ModifiedBy = modifiedBy;
        ModifiedTime = DateTime.UtcNow;
    }

    public Guid OrderId { get; set; }
    public ICollection<OrderDetailEntity> OderDetailsCollection { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTimeOffset ModifiedTime { get; set; }
}