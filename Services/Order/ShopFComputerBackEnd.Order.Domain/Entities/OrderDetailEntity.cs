using Iot.Core.Domain.Implements;
using ShopFComputerBackEnd.Order.Domain.ReadModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Iot.Core.Domain.Entities;

namespace ShopFComputerBackEnd.Order.Domain.Entities;


public class OrderDetailEntity : Entity<Guid>
{
    public OrderDetailEntity()
    {
        
    }
    public OrderDetailEntity(Guid id, Guid orderId, Guid productVariantId, int quantity, long unitPrice, bool isEnabled)
    {
        Id = id;
        OrderId = orderId;
        ProductVariantId = productVariantId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        IsEnabled = isEnabled;
    }
    [ForeignKey("Order")]
    public Guid OrderId { get; set; }
    public Guid ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public Int64 UnitPrice { get; set; }
    public bool IsEnabled { get; set; }
    public OrderReadModel Order { get; set; }
}
