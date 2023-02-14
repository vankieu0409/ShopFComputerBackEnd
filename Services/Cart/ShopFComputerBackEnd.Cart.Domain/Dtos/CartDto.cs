using Iot.Core.Domain.Implements;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using ShopFComputerBackEnd.Cart.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Cart.Domain.Dtos
{
    public class CartDto : FullAuditedEntity<Guid>
    {
        public Guid ProfileId { get; set; }
        public ICollection<CartItemValueObject> Items { get; set; }
        public ICollection<ItemsDetailsObjectValue> ItemDetails { get; set; }
    }
}
