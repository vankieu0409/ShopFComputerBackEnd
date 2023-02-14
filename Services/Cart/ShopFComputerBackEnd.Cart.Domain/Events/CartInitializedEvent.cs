using Iot.Core.Domain.Events;

using System;
using System.Collections.Generic;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;

namespace ShopFComputerBackEnd.Cart.Domain.Events;

public class CartInitializedEvent : EventBase
{
    public CartInitializedEvent(Guid id, Guid profileId, ICollection<CartItemValueObject> cartItems, DateTimeOffset createdTime)
    {
        Id = id;
        ProfileId = profileId;
        CartItems = cartItems;
        CreatedTime = createdTime;
    }
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public ICollection<CartItemValueObject> CartItems { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset ModifiedTime { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTimeOffset DeletedTime { get; set; }
}