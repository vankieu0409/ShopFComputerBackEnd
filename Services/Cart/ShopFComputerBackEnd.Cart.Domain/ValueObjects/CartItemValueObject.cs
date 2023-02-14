using System;

namespace ShopFComputerBackEnd.Cart.Domain.ReadModels
{
    public class CartItemValueObject
    {
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
