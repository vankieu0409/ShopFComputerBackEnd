using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Cart.Domain.Events
{
    public class UpdateCartItemsEvent : EventBase
    {
        public UpdateCartItemsEvent(ICollection<CartItemValueObject> cartItems, Guid modifiedBy)
        {
            CartItems = cartItems;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }

        public ICollection<CartItemValueObject> CartItems { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
