using Iot.Core.Domain.Implements;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Cart.Domain.ReadModels
{
    public class CartReadModel : FullAuditedEntity<Guid>
    {
        public Guid ProfileId { get; set; }
        public ICollection<CartItemValueObject> Items { get; set; }
    }
}
