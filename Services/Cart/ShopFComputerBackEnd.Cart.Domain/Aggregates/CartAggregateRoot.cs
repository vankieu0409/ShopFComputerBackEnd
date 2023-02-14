using System;
using System.Collections.Generic;

using Iot.Core.Domain.AggregateRoots;
using Iot.Core.Extensions;

using ShopFComputerBackEnd.Cart.Domain.Events;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;

namespace ShopFComputerBackEnd.Cart.Domain.Aggregates;

public class CartAggregateRoot : FullAggregateRoot<Guid>
{
    public CartAggregateRoot(Guid id)
    {
        if (id.IsNullOrDefault())
            throw new ArgumentNullException("Cart id");
        Id = id;
    }

    public Guid ProfileId { get; set; }
    public ICollection<CartItemValueObject> CartItems { get; set; }

    public string StreamName => $"Cart-{Id}";

    public CartAggregateRoot Initialize(Guid profileId, ICollection<CartItemValueObject> cartItems, DateTimeOffset createdTime)
    {
        if (profileId.IsNullOrDefault())
            throw new ArgumentNullException("Cart - ProfileId");
        var @event = new CartInitializedEvent(Id, profileId, cartItems, createdTime);
        AddDomainEvent(@event);
        Apply(@event);
        return this;

    }

    private void Apply(CartInitializedEvent @event)
    {
        ProfileId = @event.ProfileId;
        CartItems = @event.CartItems;
        CreatedBy = @event.CreatedBy;
        CreatedTime = @event.CreatedTime;
    }
    public CartAggregateRoot UpdateCartItems(ICollection<CartItemValueObject> cartItems,Guid modifiedBy)
    {
        var @event = new UpdateCartItemsEvent(cartItems,modifiedBy);
        AddDomainEvent(@event);
        Apply(@event);
        return this;
    }
    private void Apply(UpdateCartItemsEvent @event)
    {
        CartItems = @event.CartItems;
        ModifiedBy = @event.ModifiedBy;
        ModifiedTime = @event.ModifiedTime;
    }

}