using Iot.Core.Domain.Events;

using ShopFComputerBackEnd.Order.Domain.Entities;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace ShopFComputerBackEnd.Order.Domain.Events.Orders;

public class OrderInitializedEvent : EventBase
{
    public OrderInitializedEvent()
    {
        OrderDetailCollection = new Collection<OrderDetailEntity>();
    }
    public OrderInitializedEvent(Guid id, Guid? profileId, long amountPay, long payingCustomer, string description, long payments, int statusOrder, bool isEnabled, Guid? createdBy)
    {
        Id = id;
        ProfileId = profileId;
        AmountPay = amountPay;
        PayingCustomer = payingCustomer;
        Description = description;
        Payments = payments;
        StatusOrder = statusOrder;
        IsEnabled = isEnabled;
        CreatedBy = createdBy;
        CreateTime = DateTime.UtcNow;
    }
    public Guid Id { get; private set; }
    public Guid? ProfileId { get; private set; }
    public long AmountPay { get; private set; }
    public long PayingCustomer { get; private set; }
    public string Description { get; private set; }
    public long Payments { get; private set; }
    public int StatusOrder { get; private set; }
    public bool IsEnabled { get; private set; }
    public ICollection<OrderDetailEntity> OrderDetailCollection { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public DateTime CreateTime { get; private set; }
}