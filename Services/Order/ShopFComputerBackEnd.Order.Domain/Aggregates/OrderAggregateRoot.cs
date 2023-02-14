using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

using Iot.Core.Domain.AggregateRoots;
using Iot.Core.Extensions;

using ShopFComputerBackEnd.Order.Domain.Entities;
using ShopFComputerBackEnd.Order.Domain.Events.OrderDetails;
using ShopFComputerBackEnd.Order.Domain.Events.Orders;
using ShopFComputerBackEnd.Order.Domain.ReadModels;

namespace ShopFComputerBackEnd.Order.Domain.Aggregates;

public class OrderAggregateRoot : FullAggregateRoot<Guid>
{
    public OrderAggregateRoot(Guid id)
    {
        if (id.IsNullOrDefault())
            throw new ArgumentNullException("Order Id");
        Id = id;
    }
    public Guid? ProfileId { get; private set; }
    public long AmountPay { get; private set; }
    public long PayingCustomer { get; private set; }
    public string Description { get; private set; }
    public long Payments { get; private set; }
    public int StatusOrder { get; private set; }
    public bool IsEnabled { get; private set; }
    public ICollection<OrderDetailEntity> OrderDetailColeCollection { get; private set; }

    public string StreamName => $"Order-{Id}";

    public OrderAggregateRoot Initialize(Guid? profileId, long amountPay, long payingCustomer, string description, long payments, int statusOrder, bool isEnabled, Guid? createdBy)
    {
        var @event = new OrderInitializedEvent(Id, profileId, amountPay, payingCustomer, description, payments, statusOrder, isEnabled, createdBy);
        AddDomainEvent(@event);
        Apply(@event);
        return this;
    }

    private void Apply(OrderInitializedEvent @event)
    {
        Id = @event.Id;
        AmountPay = @event.AmountPay;
        ProfileId = @event.ProfileId;
        PayingCustomer = @event.PayingCustomer;
        Payments = @event.Payments;
        Description = @event.Description;
        StatusOrder = @event.StatusOrder;
        IsEnabled = @event.IsEnabled;
        CreatedBy = @event.CreatedBy;
        OrderDetailColeCollection = new Collection<OrderDetailEntity>();
    }

    public OrderAggregateRoot UpdateOrderDetailsCollection(ICollection<OrderDetailEntity> orderDetails, Guid orderId)
    {
        if (orderDetails.IsNullOrDefault())
            return this;
        if (orderId.IsNullOrDefault())
            return this;
        var @event = new OrderDetailsUpdateCollectionEvent(orderId, orderDetails, ModifiedBy);
        AddDomainEvent(@event);
        Apply(@event);
        return this;
    }
    private void Apply(OrderDetailsUpdateCollectionEvent @event)
    {
        Id = @event.OrderId;
        OrderDetailColeCollection = @event.OderDetailsCollection;
    }
}
